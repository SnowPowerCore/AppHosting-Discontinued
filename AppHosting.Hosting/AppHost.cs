using AppHosting.Abstractions;
using AppHosting.Abstractions.Internal;
using AppHosting.Hosting.Extensions;
using AppHosting.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AppHosting.Hosting
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHost>"/> and <see cref="IHostBuilder>"/> with pre-configured defaults.
    /// </summary>
    public class AppHost : IAppHost
    {
        private readonly IServiceCollection _applicationServiceCollection;
        private IAppStartup _startup;
        private IAppHostLifetime _applicationLifetime;
        private IAppHostedServiceExecutor _appHostedServiceExecutor;

        private readonly IServiceProvider _hostingServiceProvider;
        private readonly AppHostOptions _options;
        private readonly AggregateException _hostingStartupErrors;

        private IServiceProvider _applicationServices;
        private ILogger _logger = NullLogger.Instance;

        private bool _stopped;

        // Used for testing only
        internal AppHostOptions Options => _options;

        public IServiceProvider Services => _applicationServices;

        internal AppHost(
            IServiceCollection appServices,
            IServiceProvider hostingServiceProvider,
            AppHostOptions options,
            IConfiguration config,
            AggregateException hostingStartupErrors)
        {
            _hostingStartupErrors = hostingStartupErrors;
            _options = options;
            _applicationServiceCollection = appServices ?? throw new ArgumentNullException(nameof(appServices));
            _hostingServiceProvider = hostingServiceProvider ?? throw new ArgumentNullException(nameof(hostingServiceProvider));
            _applicationServiceCollection.AddSingleton<IAppHostLifetime, AppHostLifetime>();
            // There's no way to register multiple service types per definition. See https://github.com/aspnet/DependencyInjection/issues/360
            _applicationServiceCollection.AddSingleton(services
                => services.GetService<AppHostLifetime>() as IHostApplicationLifetime);
            _applicationServiceCollection.AddSingleton<IAppHostedServiceExecutor, AppHostedServiceExecutor>();
        }

        // Called immediately after the constructor so the properties can rely on it.
        public void Initialize()
        {
            try
            {
                EnsureApplicationServices();
            }
            catch (Exception)
            {
                // EnsureApplicationServices may have failed due to a missing or throwing Startup class.
                if (_applicationServices == null)
                {
                    _applicationServices = _applicationServiceCollection.BuildServiceProvider();
                }
            }
        }

        private void EnsureApplicationServices()
        {
            if (_applicationServices == null)
            {
                EnsureStartup();
                _applicationServices = _startup
                    .ConfigureServices(_applicationServiceCollection);
                _startup.RegisterRoutes();
            }
        }

        private void EnsureStartup()
        {
            if (_startup != null)
                return;

            _startup = _hostingServiceProvider.GetService<IAppStartup>();

            _ = _startup
                ?? throw new InvalidOperationException(
                    $"Application is not configured using any Startup class");
        }

        public TApp Start<TApp>() =>
            StartAsync<TApp>().GetAwaiter().GetResult();

        public virtual async Task<TApp> StartAsync<TApp>(CancellationToken cancellationToken = default)
        {
            _logger = _applicationServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("AppHosting.Hosting");
            _logger.ApplicationStarting();

            _applicationLifetime = _applicationServices.GetRequiredService<IAppHostLifetime>();
            _appHostedServiceExecutor = _applicationServices.GetRequiredService<IAppHostedServiceExecutor>();

            // Fire IHostedService.Start
            await _appHostedServiceExecutor
                .StartAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.ApplicationStarted();

            if (_hostingStartupErrors != null)
            {
                foreach (var exception in _hostingStartupErrors.InnerExceptions)
                {
                    _logger.HostingStartupAssemblyError(exception);
                }
            }

            return _applicationServices.GetRequiredService<TApp>();
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_stopped)
                return;
            _stopped = true;

            _logger.ApplicationShutdown();

            var timeoutToken = new CancellationTokenSource(10000).Token;
            if (!cancellationToken.CanBeCanceled)
            {
                cancellationToken = timeoutToken;
            }
            else
            {
                cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutToken).Token;
            }

            // Fire the IHostedService.Stop
            if (_appHostedServiceExecutor != null)
            {
                await _appHostedServiceExecutor
                    .StopAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            // Fire IApplicationLifetime.Stopped
            _applicationLifetime?.StopApplication();
        }

        public void Dispose() =>
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            if (!_stopped)
            {
                try
                {
                    await StopAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.ApplicationError(ex);
                }
            }

            await DisposeServiceProviderAsync(_applicationServices).ConfigureAwait(false);
            await DisposeServiceProviderAsync(_hostingServiceProvider).ConfigureAwait(false);
        }

        private async ValueTask DisposeServiceProviderAsync(IServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAppHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IAppHostBuilder>"/>.</returns>
        public static IAppHostBuilder CreateDefaultAppBuilder(string[] args)
        {
            var builder = new AppHostBuilder();

            if (string.IsNullOrEmpty(builder.GetSetting(HostDefaults.ContentRootKey)))
            {
                builder.UseContentRoot(Directory.GetCurrentDirectory());
            }

            if (args != null)
            {
                builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            builder
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;

                    config
                        .SetFileProvider(new EmbeddedFileProvider(Assembly.GetEntryAssembly()))
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = isDevelopment;
                    options.ValidateOnBuild = isDevelopment;
                });

            return builder;
        }
    }
}
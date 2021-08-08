using AppHosting.Abstractions;
using AppHosting.Abstractions.Internal;
using AppHosting.Hosting.Extensions;
using AppHosting.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace AppHosting.Hosting
{
    public class AppHostBuilder : IAppHostBuilder
    {
#nullable enable
        private readonly IConfiguration _config;

        private AppHostOptions? _options;
        private IHostEnvironment? _hostingEnvironment = new AppHostEnvironment();
        private HostBuilderContext? _context;
        private bool _mobileHostBuilt;
        private Action<HostBuilderContext, IServiceCollection>? _configureServices;
        private Action<HostBuilderContext, IConfigurationBuilder>? _configureAppConfigurationBuilder;

        private IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public AppHostBuilder()
        {
            _config = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "XAMARIN_")
                .Build();
        }

        /// <summary>
        /// Get the setting value from the configuration.
        /// </summary>
        /// <param name="key">The key of the setting to look up.</param>
        /// <returns>The value the setting currently contains.</returns>
        public string GetSetting(string key) =>
            _config[key];

        /// <summary>
        /// Add or replace a setting in the configuration.
        /// </summary>
        /// <param name="key">The key of the setting to add or replace.</param>
        /// <param name="value">The value of the setting to add or replace.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public IAppHostBuilder UseSetting(string key, string? value)
        {
            _config[key] = value;
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public IAppHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null)
                throw new ArgumentNullException(nameof(configureServices));

            return ConfigureServices((_, services) => configureServices(services));
        }

        /// <summary>
        /// Adds a delegate for configuring additional services for the host or application. This may be called
        /// multiple times.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public IAppHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
        {
            _configureServices += configureServices;
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder" /> that will be used to construct an <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        /// <remarks>
        /// The <see cref="IConfiguration"/> and <see cref="ILoggerFactory"/> on the <see cref="HostBuilderContext"/> are uninitialized at this stage.
        /// The <see cref="IConfigurationBuilder"/> is pre-populated with the settings of the <see cref="IAppHostBuilder"/>.
        /// </remarks>
        public IAppHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppConfigurationBuilder += configureDelegate;
            return this;
        }

        public IAppHost Build()
        {
            if (_mobileHostBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            _mobileHostBuilt = true;

            _hostingEnvironment = new AppHostEnvironment();
            _context = new HostBuilderContext(Properties)
            {
                HostingEnvironment = _hostingEnvironment,
            };
            var hostingServices = BuildCommonServices(out var hostingStartupErrors);
            var applicationServices = hostingServices.Clone();
            var hostingServiceProvider = GetProviderFromFactory(hostingServices);

            AddApplicationServices(applicationServices, hostingServiceProvider);

            var host = new AppHost(
                applicationServices,
                hostingServiceProvider,
                _options,
                _config,
                hostingStartupErrors);
            try
            {
                host.Initialize();
                // resolve configuration explicitly once to mark it as resolved within the
                // service provider, ensuring it will be properly disposed with the provider
                _ = host.Services.GetService<IConfiguration>();

                var logger = host.Services.GetRequiredService<ILogger<AppHost>>();

                return host;
            }
            catch
            {
                // Dispose the host if there's a failure to initialize, this should dispose
                // services that were constructed until the exception was thrown
                host.Dispose();
                throw;
            }

            static IServiceProvider GetProviderFromFactory(IServiceCollection collection)
            {
                var provider = collection.BuildServiceProvider();
                var factory = provider.GetService<IServiceProviderFactory<IServiceCollection>>();

                if (factory != null && !(factory is DefaultServiceProviderFactory))
                    using (provider)
                        return factory.CreateServiceProvider(factory.CreateBuilder(collection));

                return provider;
            }
        }

        private IServiceCollection BuildCommonServices(out AggregateException? hostingStartupErrors)
        {
            hostingStartupErrors = default;

            _options = new AppHostOptions(_config);

            var services = new ServiceCollection();

            services
                .AddSingleton(_options)
                .AddSingleton(_hostingEnvironment)
                .AddSingleton(_context);

            var builder = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddConfiguration(_config, shouldDisposeConfiguration: true);

            _configureAppConfigurationBuilder?.Invoke(_context, builder);

            var configuration = builder.Build();
            // register configuration as factory to make it dispose with the service provider
            services.AddSingleton<IConfiguration>(_ => configuration);
            _context.Configuration = configuration;

            var listener = new DiagnosticListener("AppHosting");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);

            services.AddOptions();
            services.AddLogging();

            services.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();

            try
            {
                var startupType = StartupLoader
                    .FindStartupType(
                        GetSetting(HostDefaults.ApplicationKey),
                        _hostingEnvironment.EnvironmentName);

                if (typeof(IAppStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                {
                    services.AddSingleton(typeof(IAppStartup), startupType);
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                var capture = ExceptionDispatchInfo.Capture(ex);
                services.AddSingleton<IAppStartup>(_ =>
                {
                    capture.Throw();
                    return default;
                });
            }

            _configureServices?.Invoke(_context, services);

            return services;
        }

        private void AddApplicationServices(
            IServiceCollection services, IServiceProvider hostingServiceProvider)
        {
            // We are forwarding services from hosting container so hosting container
            // can still manage their lifetime (disposal) shared instances with application services.
            // NOTE: This code overrides original services lifetime. Instances would always be singleton in
            // application container.
            var listener = hostingServiceProvider.GetService<DiagnosticListener>();
            services.Replace(ServiceDescriptor.Singleton(typeof(DiagnosticListener), listener!));
            services.Replace(ServiceDescriptor.Singleton(typeof(DiagnosticSource), listener!));
        }
    }
#nullable disable
}
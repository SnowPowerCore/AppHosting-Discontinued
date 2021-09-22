using AppHosting.Abstractions.Interfaces;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace AppHosting.Hosting.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IHostBuilder"/>.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Specify the startup type to be used by the app host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <param name="startupType">The <see cref="Type"/> to be used.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseStartup(this IAppHostBuilder hostBuilder, Type startupType)
        {
            var startupAssemblyName = startupType.Assembly.GetName().Name;

            hostBuilder.UseSetting(HostDefaults.ApplicationKey, startupAssemblyName);

            return hostBuilder
                .ConfigureServices(services =>
                {
                    if (typeof(IAppStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton(typeof(IAppStartup), startupType);
                    }
                    else
                    {
                        //
                    }
                });
        }

        /// <summary>
        /// Specify the startup type to be used by the app host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <typeparam name ="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseStartup<TStartup>(this IAppHostBuilder hostBuilder) where TStartup : class =>
            hostBuilder.UseStartup(typeof(TStartup));

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder ConfigureLogging(
            this IAppHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging) =>
            hostBuilder.ConfigureServices(collection => collection.AddLogging(configureLogging));

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="LoggerFactory"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="LoggerFactory"/>.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder ConfigureLogging(
            this IAppHostBuilder hostBuilder, Action<HostBuilderContext, ILoggingBuilder> configureLogging) =>
            hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));

        /// <summary>
        /// Configures the default service provider
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseDefaultServiceProvider(
            this IAppHostBuilder hostBuilder, Action<HostBuilderContext, ServiceProviderOptions> configure) =>
            hostBuilder.ConfigureServices((context, services) =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                services.Replace(ServiceDescriptor
                    .Singleton<IServiceProviderFactory<IServiceCollection>>(
                        new DefaultServiceProviderFactory(options)));
            });

        /// <summary>
        /// Use the given configuration settings on the app host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing settings to be used.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseConfiguration(
            this IAppHostBuilder hostBuilder, IConfiguration configuration)
        {
            foreach (var setting in configuration.AsEnumerable(makePathsRelative: true))
                hostBuilder.UseSetting(setting.Key, setting.Value);

            return hostBuilder;
        }

        /// <summary>
        /// Specify the content root directory to be used by the app host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <param name="contentRoot">Path to root directory of the application.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseContentRoot(this IAppHostBuilder hostBuilder, string contentRoot)
        {
            if (contentRoot == null)
                throw new ArgumentNullException(nameof(contentRoot));

            return hostBuilder.UseSetting(HostDefaults.ContentRootKey, contentRoot);
        }

        /// <summary>
        /// Specify the environment to be used by the app host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <param name="contentRoot">Path to root directory of the application.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseEnvironment(this IAppHostBuilder hostBuilder, string environmentName)
        {
            if (string.IsNullOrEmpty(environmentName))
                throw new ArgumentNullException(nameof(environmentName));

            return hostBuilder.UseSetting(HostDefaults.EnvironmentKey, environmentName);
        }
    }
}
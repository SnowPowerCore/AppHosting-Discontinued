using AppHosting.Abstractions.Interfaces;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Factory;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AppHosting.Xamarin.Forms.Internal.Factory;
using AppHosting.Xamarin.Forms.Services.Navigation;
using AppHosting.Xamarin.Forms.Services.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Assigns app visual builder.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseAppVisualProcessor(this IAppHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IPageBuilderFactory, PageBuilderFactory>();
                    services.AddSingleton<IElementBuilderFactory, ElementBuilderFactory>();
                    services.AddSingleton<IAppVisualProcessor, AppVisualProcessor>();
                });

        /// <summary>
        /// Tells the application to use shell navigation as a main navigation service.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseShellNavigation(this IAppHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureServices(services =>
                    services.AddSingleton<INavigationService, ShellNavigationService>());

        /// <summary>
        /// Tells the application to use legacy tabbed navigation as a main navigation service.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IAppHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IAppHostBuilder"/>.</returns>
        public static IAppHostBuilder UseLegacyTabbedNavigation(this IAppHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureServices(services =>
                    services.AddSingleton<INavigationService, LegacyTabbedNavigationService>());
    }
}
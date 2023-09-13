using AppHosting.Xamarin.Forms.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVisualProcessingCore(this IServiceCollection services)
        {
            services.AddSingleton<BindingContextMiddleware>();
            services.AddSingleton<ChildrenBindingContextMiddleware>();
            services.AddSingleton<PageAppearingMiddleware>();
            services.AddSingleton<PageDisappearingMiddleware>();
            services.AddSingleton<ProcessElementMiddleware>();
            services.AddSingleton<CommandMiddleware>();
            services.AddSingleton<AsyncCommandMiddleware>();
            services.AddSingleton<AttachedCommandMiddleware>();
            services.AddSingleton<AttachedAsyncCommandMiddleware>();
            services.AddSingleton<AttachedLongPressCommandMiddleware>();
            services.AddSingleton<AttachedAsyncLongPressCommandMiddleware>();
            return services;
        }
    }
}
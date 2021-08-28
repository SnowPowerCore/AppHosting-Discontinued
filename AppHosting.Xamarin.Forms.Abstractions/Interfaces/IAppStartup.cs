using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces
{
    public interface IAppStartup
    {
        void ConfigureServices(IServiceCollection services);

        void ConfigurePage(IPageBuilder pageBuilder);

        void ConfigureElement(IElementBuilder elementBuilder);

        void RegisterRoutes();
    }
}
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IPageNavigation
    {
        void DetermineAndSetMainPage<TPage>();

        Task SwitchMainPageAsync<TPage>(TPage page);

        Task NavigateToPageAsync(string routeWithParams, bool animated = true);

        Task NavigateBackAsync(bool animated = true);

        Task NavigateToRootAsync(bool animated = true);
    }
}
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IPopupNavigation
    {
        Task OpenPopupAsync(string routeWithParams, bool animated = true);

        Task ClosePopupAsync(bool animated = true);
    }
}
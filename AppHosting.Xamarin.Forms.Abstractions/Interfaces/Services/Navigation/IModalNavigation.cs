using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IModalNavigation
    {
        Task OpenModalAsync<TPage>(TPage modal, bool animated = true);

        Task CloseModalAsync(bool animated = true);
    }
}
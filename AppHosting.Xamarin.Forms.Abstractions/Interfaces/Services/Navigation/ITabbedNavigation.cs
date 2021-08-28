using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface ITabbedNavigation
    {
        Task SwitchItemAsync(int index);
    }
}
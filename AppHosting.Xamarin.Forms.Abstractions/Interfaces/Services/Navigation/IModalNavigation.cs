using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using NavigationEventArgs = AppHosting.Xamarin.Forms.Abstractions.EventArgs.NavigationEventArgs;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IModalNavigation
    {
        event EventHandler<NavigationEventArgs> ModalNavigating;

        IReadOnlyList<Page> Modals { get; }

        Task OpenModalAsync<TPage>(TPage modal, bool animated = true);

        Task CloseModalAsync(bool animated = true);
    }
}
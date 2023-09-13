using AppHosting.Xamarin.Forms.Abstractions.EventArgs;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IPopupNavigation
    {
        event EventHandler<NavigationEventArgs> PopupNavigating;

        IReadOnlyList<PopupPage> PopupPages { get; }

        Task OpenPopupAsync(string routeWithParams, bool animated = true);

        Task ClosePopupAsync(bool animated = true);
    }
}
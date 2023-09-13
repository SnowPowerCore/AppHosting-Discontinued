using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using NavigationEventArgs = AppHosting.Xamarin.Forms.Abstractions.EventArgs.NavigationEventArgs;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation
{
    public interface IPageNavigation
    {
        event EventHandler<NavigationEventArgs> PageNavigating;

        IReadOnlyList<Page> Pages { get; }

        void DetermineAndSetMainPage<TPage>();

        Task SwitchMainPageAsync<TPage>(TPage page);

        Task NavigateToPageAsync(string routeWithParams, bool animated = true);

        Task NavigateBackAsync(bool animated = true);

        Task NavigateToRootAsync(bool animated = true);
    }
}
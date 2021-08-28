using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Navigation;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AppHosting.Xamarin.Forms.Controls;
using AppHosting.Xamarin.Forms.Extensions;
using AsyncAwaitBestPractices;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Services.Navigation
{
    public class ShellNavigationService : INavigationService
    {
        private readonly List<Guid> _processedItems = new();

        private readonly IServiceProvider _serviceProvider;
        private readonly IAppVisualProcessor _appVisualProcessor;

        public ShellNavigationService(IServiceProvider serviceProvider,
                                      IAppVisualProcessor appVisualProcessor)
        {
            _serviceProvider = serviceProvider;
            _appVisualProcessor = appVisualProcessor;
        }

        public Task SwitchMainPageAsync<TPage>(TPage page)
        {
            if (page is Shell shell)
            {
                Shell.Current.FlyoutIsPresented = false;
                ProcessPageAsync(shell).SafeFireAndForget();
                return CloseModalAsync()
                    .ContinueWith(t =>
                        Device.InvokeOnMainThreadAsync(() =>
                            Application.Current.MainPage = shell),
                                TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            return Task.CompletedTask;
        }

        public void DetermineAndSetMainPage<TPage>()
        {
            if (typeof(TPage).IsSubclassOf(typeof(Shell)))
            {
                var shell = (HostedShell)_serviceProvider.GetService(typeof(TPage));
                ProcessPageAsync(shell).SafeFireAndForget();
                Application.Current.MainPage = shell;
            }
        }

        public Task OpenModalAsync<TPage>(TPage modal, bool animated = true)
        {
            if (modal is Page xfModal)
            {
                ProcessPageAsync(xfModal).SafeFireAndForget();
                return Device.InvokeOnMainThreadAsync(
                    () => Shell.Current.Navigation.PushModalAsync(xfModal, animated));
            }
            return Task.CompletedTask;
        }

        public Task CloseModalAsync(bool animated = true) =>
            Shell.Current.Navigation.ModalStack.Count > 0
                ? Device.InvokeOnMainThreadAsync(
                    () => Shell.Current.Navigation.PopModalAsync(animated))
                : Task.CompletedTask;

        public Task OpenPopupAsync(string routeWithParams, bool animated = true)
        {
            var popupPage = routeWithParams.GetElementFromRouting<PopupPage>();
            ProcessPageAsync(popupPage).SafeFireAndForget();
            return Device.InvokeOnMainThreadAsync(
                () => PopupNavigation.Instance.PushAsync(popupPage, animated));
        }

        public Task ClosePopupAsync(bool animated = true) =>
            PopupNavigation.Instance.PopupStack.Count > 0
                ? Device.InvokeOnMainThreadAsync(
                    () => PopupNavigation.Instance.PopAsync(animated))
                : Task.CompletedTask;

        public Task NavigateToPageAsync(string routeWithParams, bool animated = true)
        {
            Shell.Current.FlyoutIsPresented = false;
            var page = routeWithParams.GetElementFromRouting<Page>();
            ProcessPageAsync(page).SafeFireAndForget();
            return Device.InvokeOnMainThreadAsync(
                () => Shell.Current.Navigation.PushAsync(page, animated));
        }

        public Task NavigateBackAsync(bool animated = true) =>
            Shell.Current.Navigation.NavigationStack.Count > 1
                ? Device.InvokeOnMainThreadAsync(
                    () => Shell.Current.Navigation.PopAsync(animated))
                : Task.CompletedTask;

        public Task NavigateToRootAsync(bool animated = true)
        {
            Shell.Current.FlyoutIsPresented = false;
            return Device.InvokeOnMainThreadAsync(
                () => Shell.Current.Navigation.PopToRootAsync(animated));
        }

        public Task SwitchItemAsync(int index) =>
            Device.InvokeOnMainThreadAsync(
                () => Shell.Current.CurrentItem = Shell.Current.Items.ElementAtOrDefault(index));

        private Task ProcessPageAsync(Page page) =>
            Device.InvokeOnMainThreadAsync(() =>
            {
                if (_processedItems.Contains(page.Id))
                    return Task.CompletedTask;
                var elementTask = _appVisualProcessor.ElementProcessing?.Invoke(page);
                var pageTask = _appVisualProcessor.PageProcessing?.Invoke(page);
                _processedItems.Add(page.Id);
                return Task.WhenAll(elementTask, pageTask);
            });
    }
}
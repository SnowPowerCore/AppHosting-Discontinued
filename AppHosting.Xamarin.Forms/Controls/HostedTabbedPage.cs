using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AsyncAwaitBestPractices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Controls
{
    public class HostedTabbedPage : TabbedPage
    {
        private readonly List<Guid> _processedTabItems = new();

        private readonly IAppVisualProcessor _appVisualProcessor;

        private HostedTabbedPage() : base() { }

        public HostedTabbedPage(IAppVisualProcessor appVisualProcessor) : base()
        {
            _appVisualProcessor = appVisualProcessor;
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is NavigationPage navigationPage)
                ProcessPageAsync(navigationPage.CurrentPage).SafeFireAndForget();
            else if (child is Page page)
                ProcessPageAsync(page).SafeFireAndForget();
        }

        private Task ProcessPageAsync(Page page) =>
            Device.InvokeOnMainThreadAsync(() =>
            {
                if (_processedTabItems.Contains(page.Id))
                    return Task.CompletedTask;
                var elementTask = _appVisualProcessor.ElementProcessing?.Invoke(page);
                var pageTask = elementTask
                    .ContinueWith(t => _appVisualProcessor.PageProcessing?.Invoke(page),
                        TaskContinuationOptions.OnlyOnRanToCompletion);
                _processedTabItems.Add(page.Id);
                return pageTask.Unwrap();
            });
    }
}
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AppHosting.Xamarin.Forms.Extensions;
using AsyncAwaitBestPractices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Controls
{
    public class HostedShell : Shell
    {
        private readonly List<Guid> _processedShellItems = new();

        private readonly IAppVisualProcessor _appVisualProcessor;

        private HostedShell() : base() { }

        public HostedShell(IAppVisualProcessor pageProcessor) : base()
        {
            _appVisualProcessor = pageProcessor;
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            var dest = args.Target.Location.OriginalString.GetDestinationRoute();

            var destShellContent = (ShellContent)FindByName(dest);
            if (destShellContent is default(ShellContent))
            {
                base.OnNavigating(args);
                return;
            }

            var currentPage = (Page)destShellContent.ContentTemplate.CreateContent();
            if (currentPage != default)
            {
                Device
                    .InvokeOnMainThreadAsync(() =>
                    {
                        if (_processedShellItems.Contains(currentPage.Id))
                            return Task.CompletedTask;
                        var elementTask = _appVisualProcessor.ElementProcessing?.Invoke(currentPage);
                        var pageTask = elementTask
                            .ContinueWith(t => _appVisualProcessor.PageProcessing?.Invoke(currentPage),
                                TaskContinuationOptions.OnlyOnRanToCompletion);
                        _processedShellItems.Add(currentPage.Id);
                        return pageTask.Unwrap();
                    })
                    .SafeFireAndForget();
            }

            base.OnNavigating(args);
        }
    }
}
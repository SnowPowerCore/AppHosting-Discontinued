using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AppHosting.Xamarin.Forms.Extensions;
using AsyncAwaitBestPractices;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Controls
{
    public class HostedShell : Shell
    {
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
                        _appVisualProcessor.PageProcessing?.Invoke(currentPage);
                        _appVisualProcessor.ElementProcessing?.Invoke(currentPage);
                    })
                    .SafeFireAndForget();
            }

            base.OnNavigating(args);
        }
    }
}
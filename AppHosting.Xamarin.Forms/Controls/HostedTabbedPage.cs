using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AsyncAwaitBestPractices;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Controls
{
    public class HostedTabbedPage : TabbedPage
    {
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
            {
                Device
                    .InvokeOnMainThreadAsync(() =>
                    {
                        _appVisualProcessor.PageProcessing?.Invoke(navigationPage.CurrentPage);
                        _appVisualProcessor.ElementProcessing?.Invoke(navigationPage.CurrentPage);
                    })
                    .SafeFireAndForget();
            }
            else if (child is Page page)
            {
                Device
                    .InvokeOnMainThreadAsync(() =>
                    {
                        _appVisualProcessor.PageProcessing?.Invoke(page);
                        _appVisualProcessor.ElementProcessing?.Invoke(page);
                    })
                    .SafeFireAndForget();
            }
        }
    }
}
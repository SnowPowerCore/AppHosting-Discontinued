using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class ProcessElementMiddleware : IPageMiddleware
    {
        private readonly IAppVisualProcessor _appVisualProcessor;

        public ProcessElementMiddleware(IAppVisualProcessor appVisualProcessor)
        {
            _appVisualProcessor = appVisualProcessor;
        }

        public Task InvokeAsync(Page page, PageDelegate next)
        {
            var processElementAttrs = page.GetProcessElementsAttributes();
            foreach (var attr in processElementAttrs)
            {
                if (string.IsNullOrEmpty(attr.ControlName))
                    continue;

                var desiredControl = page.GetControlData(attr.ControlName, out _, out _);

                _appVisualProcessor.ElementProcessing?.Invoke((Element)desiredControl);
            }
            return next(page);
        }
    }
}
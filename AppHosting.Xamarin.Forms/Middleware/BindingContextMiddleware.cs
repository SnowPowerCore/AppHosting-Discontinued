using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class BindingContextMiddleware : IElementMiddleware
    {
        private readonly IServiceProvider _services;

        public BindingContextMiddleware(IServiceProvider services)
        {
            _services = services;
        }

        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var bindingContextAttrs = element.GetElementBindingContextAttributes();
            var xfElementType = element.GetType();

            var elementBindingContext = bindingContextAttrs.LastOrDefault(
                x => string.IsNullOrEmpty(x.ControlName));

            if (elementBindingContext != default)
                element.BindingContext = _services.GetService(elementBindingContext.BindingContextType);

            return next(element);
        }
    }
}
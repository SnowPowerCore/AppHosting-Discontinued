using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class ChildrenBindingContextMiddleware : IElementMiddleware
    {
        private readonly IServiceProvider _services;

        public ChildrenBindingContextMiddleware(IServiceProvider services)
        {
            _services = services;
        }

        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var bindingContextAttrs = element.GetElementBindingContextAttributes();
            var xfElementType = element.GetType();

            var childrenBindingContexts = bindingContextAttrs
                .Where(x => !string.IsNullOrEmpty(x.ControlName));

            _ = childrenBindingContexts.Select(x =>
            {
                var field = xfElementType.GetField(x.ControlName,
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (field is default(FieldInfo))
                    return default;

                if (field.FieldType.IsSubclassOf(typeof(BindableObject)))
                {
                    var bindableObj = (BindableObject)field.GetValue(element);
                    var bindingContext = _services.GetService(x.BindingContextType);
                    bindableObj.BindingContext = bindingContext;
                }

                return x;
            });

            return next(element);
        }
    }
}
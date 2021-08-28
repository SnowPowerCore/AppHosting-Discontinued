using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class AsyncCommandMiddleware : IElementMiddleware
    {
        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var commandAttrs = element.GetElementAsyncCommandAttributes();
            var updatedElement = element.AddAsyncCommands(commandAttrs);
            return next(updatedElement);
        }
    }
}
using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class CommandMiddleware : IElementMiddleware
    {
        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var commandAttrs = element.GetElementCommandAttributes();
            var updatedElement = element.AddCommands(commandAttrs);
            return next(updatedElement);
        }
    }
}
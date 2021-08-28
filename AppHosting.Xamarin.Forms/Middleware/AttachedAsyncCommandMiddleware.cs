using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class AttachedAsyncCommandMiddleware : IElementMiddleware
    {
        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var commandAttrs = element.GetElementAttachedAsyncCommandAttributes();
            var updatedElement = element.AddAttachedAsyncCommands(commandAttrs);
            return next(updatedElement);
        }
    }
}
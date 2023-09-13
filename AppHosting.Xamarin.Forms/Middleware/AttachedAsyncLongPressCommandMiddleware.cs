using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class AttachedAsyncLongPressCommandMiddleware : IElementMiddleware
    {
        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var commandAttrs = element.GetElementAttachedAsyncLongPressCommandAttributes();
            var updatedElement = element.AddAttachedAsyncLongPressCommands(commandAttrs);
            return next(updatedElement);
        }
    }
}
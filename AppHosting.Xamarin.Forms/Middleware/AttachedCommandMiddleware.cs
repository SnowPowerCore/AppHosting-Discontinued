using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class AttachedCommandMiddleware : IElementMiddleware
    {
        public Task InvokeAsync(Element element, ElementDelegate next)
        {
            var attachedCommandAttrs = element.GetElementAttachedCommandAttributes();
            var updatedElement = element.AddAttachedCommands(attachedCommandAttrs);
            return next(updatedElement);
        }
    }
}
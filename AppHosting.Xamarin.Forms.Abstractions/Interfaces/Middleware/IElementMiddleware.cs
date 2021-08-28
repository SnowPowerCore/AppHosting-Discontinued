using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware
{
    public interface IElementMiddleware
    {
        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="element">Current <see cref="Element"/>.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
        Task InvokeAsync(Element element, ElementDelegate next);
    }
}
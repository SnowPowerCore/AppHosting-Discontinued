using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware
{
    /// <summary>
    /// Defines middleware that can be added to the application's page processing pipeline.
    /// </summary>
    public interface IPageMiddleware
    {
        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
        Task InvokeAsync(Page page, PageDelegate next);
    }
}
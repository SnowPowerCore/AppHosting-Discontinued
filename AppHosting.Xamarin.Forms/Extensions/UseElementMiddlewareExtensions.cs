using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AppHosting.Xamarin.Forms.Extensions
{
    /// <summary>
    /// Extension methods for adding typed middleware.
    /// </summary>
    public static class UseElementMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type.</typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IElementBuilder UseMiddleware<TMiddleware>(this IElementBuilder app, params object[] args) =>
            app.UseMiddleware(typeof(TMiddleware), args);

        /// <summary>
        /// Adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IPageBuilder"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IElementBuilder UseMiddleware(this IElementBuilder app, Type middleware, params object[] args)
        {
            if (typeof(IElementMiddleware).GetTypeInfo().IsAssignableFrom(middleware.GetTypeInfo()))
            {
                // IMiddleware doesn't support passing args directly since it's
                // activated from the container
                if (args.Length > 0)
                {
                    throw new NotSupportedException(
                        "Cannot instantiate middleware from outside of the container");
                }

                return UseMiddlewareInterface(app, app.ApplicationServices, middleware);
            }

            throw new InvalidOperationException("Middleware should inherit IPageMiddleware");
        }

        private static IElementBuilder UseMiddlewareInterface(IElementBuilder app, IServiceProvider services, Type middlewareType) =>
            app.Use(next =>
                context =>
                {
                    var middleware = (IElementMiddleware)services.GetRequiredService(middlewareType);
                    if (middleware == null)
                    {
                        //It's a broken implementation
                        throw new InvalidOperationException(
                            "It's a broken implementation");
                    }
                    return middleware.InvokeAsync(context, next);
                });
    }
}
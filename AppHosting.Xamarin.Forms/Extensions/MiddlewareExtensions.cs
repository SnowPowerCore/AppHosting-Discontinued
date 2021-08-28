using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using AppHosting.Xamarin.Forms.Middleware;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IElementBuilder AssignBindingContext(this IElementBuilder app) =>
            app.UseMiddleware<BindingContextMiddleware>();

        public static IElementBuilder AssignChildrenBindingContext(this IElementBuilder app) =>
            app.UseMiddleware<ChildrenBindingContextMiddleware>();

        public static IElementBuilder AssignCommands(this IElementBuilder app) =>
            app.UseMiddleware<CommandMiddleware>();

        public static IElementBuilder AssignAsyncCommands(this IElementBuilder app) =>
            app.UseMiddleware<AsyncCommandMiddleware>();

        public static IElementBuilder AssignAttachedCommands(this IElementBuilder app) =>
            app.UseMiddleware<AttachedCommandMiddleware>();

        public static IElementBuilder AssignAttachedAsyncCommands(this IElementBuilder app) =>
            app.UseMiddleware<AttachedAsyncCommandMiddleware>();

        public static IPageBuilder AssignPageAppearing(this IPageBuilder app) =>
            app.UseMiddleware<PageAppearingMiddleware>();

        public static IPageBuilder AssignPageDisappearing(this IPageBuilder app) =>
            app.UseMiddleware<PageDisappearingMiddleware>();

        public static IPageBuilder ProcessPageElements(this IPageBuilder app) =>
            app.UseMiddleware<ProcessElementMiddleware>();
    }
}
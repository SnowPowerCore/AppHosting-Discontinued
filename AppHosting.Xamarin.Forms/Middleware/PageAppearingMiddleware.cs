using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Attributes;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class PageAppearingMiddleware : IPageMiddleware
    {
        public Task InvokeAsync(Page page, PageDelegate next)
        {
            var pageAppearingAttr = page.GetElementPageAppearingAttribute();
            if (pageAppearingAttr is default(PageAppearingAttribute))
                return next(page);

            if (string.IsNullOrEmpty(pageAppearingAttr.PageAppearingTaskName))
                return next(page);

            var methodInvoke = page.BindingContext.GetPageActionFromBindingContext(
                pageAppearingAttr.PageAppearingTaskName);

            page.Appearing += (o, e) => methodInvoke();

            return next(page);
        }
    }
}
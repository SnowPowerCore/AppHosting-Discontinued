using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Middleware;
using AppHosting.Xamarin.Forms.Attributes;
using AppHosting.Xamarin.Forms.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Middleware
{
    public class PageDisappearingMiddleware : IPageMiddleware
    {
        public Task InvokeAsync(Page page, PageDelegate next)
        {
            var pageDisappearingAttr = page.GetElementPageDisappearingAttribute();
            if (pageDisappearingAttr is default(PageDisappearingAttribute))
                return next(page);

            if (string.IsNullOrEmpty(pageDisappearingAttr.PageDisappearingTaskName))
                return next(page);

            var methodInvoke = page.BindingContext.GetPageActionFromBindingContext(
                pageDisappearingAttr.PageDisappearingTaskName);

            page.Disappearing += (o, e) => methodInvoke();

            return next(page);
        }
    }
}
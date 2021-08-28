using AsyncAwaitBestPractices;
using System;
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class PageExtensions
    {
        public static Action GetPageActionFromBindingContext(this object bindingContext, string taskName)
        {
            var method = bindingContext
                .GetType()
                .GetMethod(taskName);

            return () => ((Task)method.Invoke(bindingContext, Array.Empty<object>()))
                .SafeFireAndForget();
        }
    }
}
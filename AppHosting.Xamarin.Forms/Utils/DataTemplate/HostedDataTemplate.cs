using AppHosting.Xamarin.Forms.Extensions;
using AppHosting.Xamarin.Forms.Utils.Wrappers;
using System;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Utils.DataTemplate
{
    public class HostedDataTemplate : global::Xamarin.Forms.DataTemplate
    {
        public HostedDataTemplate() { }

        public HostedDataTemplate(TypeWrapper typeWrapper) : base(() =>
        {
            var type = typeWrapper.Type;
            var data = Activator.CreateInstance(type);
            if (data is Element xfElement)
            {
                var commandAttrs = xfElement.GetElementCommandAttributes();
                var asyncCommandAttrs = xfElement.GetElementAsyncCommandAttributes();
                var attachedCommandAttrs = xfElement.GetElementAttachedCommandAttributes();
                var attachedLongPressCommandAttrs = xfElement.GetElementAttachedLongPressCommandAttributes();
                var attachedAsyncCommandAttrs = xfElement.GetElementAttachedAsyncCommandAttributes();
                var attachedAsyncLongPressCommandAttrs = xfElement.GetElementAttachedAsyncLongPressCommandAttributes();

                var parentBindingContext = typeWrapper.Parent.BindingContext;
                data = xfElement
                    .AddAttachedAsyncCommands(attachedAsyncCommandAttrs, parentBindingContext)
                    .AddAttachedCommands(attachedCommandAttrs, parentBindingContext)
                    .AddAttachedAsyncLongPressCommands(attachedAsyncLongPressCommandAttrs, parentBindingContext)
                    .AddAttachedLongPressCommands(attachedLongPressCommandAttrs, parentBindingContext)
                    .AddAsyncCommands(asyncCommandAttrs, parentBindingContext)
                    .AddCommands(commandAttrs, parentBindingContext);
            }
            return data;
        })
        { }
    }
}
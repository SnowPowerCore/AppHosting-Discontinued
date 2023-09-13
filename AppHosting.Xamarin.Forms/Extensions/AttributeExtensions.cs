using AppHosting.Xamarin.Forms.Attributes;
using System;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class AttributeExtensions
    {
        public static BindingContextAttribute[] GetElementBindingContextAttributes(this Element xfElement)
        {
            var bindingContextAttrs = (BindingContextAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(BindingContextAttribute));

            return bindingContextAttrs is default(BindingContextAttribute[])
                ? Array.Empty<BindingContextAttribute>()
                : bindingContextAttrs;
        }

        public static CommandAttribute[] GetElementCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (CommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(CommandAttribute),
                true);

            return commandAttrs is default(CommandAttribute[])
                ? Array.Empty<CommandAttribute>()
                : commandAttrs;
        }

        public static AttachedCommandAttribute[] GetElementAttachedCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (AttachedCommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(AttachedCommandAttribute),
                true);

            return commandAttrs is default(AttachedCommandAttribute[])
                ? Array.Empty<AttachedCommandAttribute>()
                : commandAttrs;
        }

        public static AttachedLongPressCommandAttribute[] GetElementAttachedLongPressCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (AttachedLongPressCommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(AttachedLongPressCommandAttribute),
                true);

            return commandAttrs is default(AttachedLongPressCommandAttribute[])
                ? Array.Empty<AttachedLongPressCommandAttribute>()
                : commandAttrs;
        }

        public static AsyncCommandAttribute[] GetElementAsyncCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (AsyncCommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(AsyncCommandAttribute),
                true);

            return commandAttrs is default(AsyncCommandAttribute[])
                ? Array.Empty<AsyncCommandAttribute>()
                : commandAttrs;
        }

        public static AttachedAsyncCommandAttribute[] GetElementAttachedAsyncCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (AttachedAsyncCommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(AttachedAsyncCommandAttribute),
                true);

            return commandAttrs is default(AttachedAsyncCommandAttribute[])
                ? Array.Empty<AttachedAsyncCommandAttribute>()
                : commandAttrs;
        }

        public static AttachedAsyncLongPressCommandAttribute[] GetElementAttachedAsyncLongPressCommandAttributes(this Element xfElement)
        {
            var commandAttrs = (AttachedAsyncLongPressCommandAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(AttachedAsyncLongPressCommandAttribute),
                true);

            return commandAttrs is default(AttachedAsyncLongPressCommandAttribute[])
                ? Array.Empty<AttachedAsyncLongPressCommandAttribute>()
                : commandAttrs;
        }

        public static ProcessElementAttribute[] GetProcessElementsAttributes(this Element xfElement)
        {
            var bindingContextAttrs = (ProcessElementAttribute[])Attribute.GetCustomAttributes(
                xfElement.GetType(),
                typeof(ProcessElementAttribute));

            return bindingContextAttrs is default(ProcessElementAttribute[])
                ? Array.Empty<ProcessElementAttribute>()
                : bindingContextAttrs;
        }

        public static PageAppearingAttribute GetElementPageAppearingAttribute(this Page xfPage)
        {
            var pageAppearingAttr = (PageAppearingAttribute)Attribute.GetCustomAttribute(
                xfPage.GetType(),
                typeof(PageAppearingAttribute));

            return pageAppearingAttr is default(PageAppearingAttribute)
                ? default
                : pageAppearingAttr;
        }

        public static PageDisappearingAttribute GetElementPageDisappearingAttribute(this Page xfPage)
        {
            var pageAppearingAttr = (PageDisappearingAttribute)Attribute.GetCustomAttribute(
                xfPage.GetType(),
                typeof(PageDisappearingAttribute));

            return pageAppearingAttr is default(PageDisappearingAttribute)
                ? default
                : pageAppearingAttr;
        }
    }
}
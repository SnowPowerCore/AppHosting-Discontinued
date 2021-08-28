using System;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class ControlExtensions
    {
        public static BindableObject GetControlData<TElement>(
            this TElement element, string controlName, out Type controlType, out Type bindingContextType)
            where TElement : Element
        {
            var desiredControl = (BindableObject)element;
            if (!string.IsNullOrEmpty(controlName))
                desiredControl = (BindableObject)element.FindByName(controlName);
            controlType = desiredControl.GetType();
            bindingContextType = element.BindingContext?.GetType();

            return desiredControl;
        }
    }
}
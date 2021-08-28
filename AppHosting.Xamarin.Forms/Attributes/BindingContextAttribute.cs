using System;

namespace AppHosting.Xamarin.Forms.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BindingContextAttribute : Attribute
    {
        public Type BindingContextType { get; } = typeof(object);

        public string ControlName { get; } = string.Empty;

        public BindingContextAttribute(Type bindingContextType)
        {
            BindingContextType = bindingContextType;
        }

        public BindingContextAttribute(string controlName, Type bindingContextType)
        {
            ControlName = controlName;
            BindingContextType = bindingContextType;
        }
    }
}
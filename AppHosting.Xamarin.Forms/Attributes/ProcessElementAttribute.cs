using System;

namespace AppHosting.Xamarin.Forms.Attributes
{
    /// <summary>
    /// Please, use it inside of a page class for any element. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProcessElementAttribute : Attribute
    {
        public string ControlName { get; }

        public ProcessElementAttribute(string controlName)
        {
            ControlName = controlName;
        }
    }
}
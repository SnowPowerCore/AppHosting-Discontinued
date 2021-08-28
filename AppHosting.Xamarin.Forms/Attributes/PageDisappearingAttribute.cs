using System;

namespace AppHosting.Xamarin.Forms.Attributes
{
    /// <summary>
    /// Please, use it on a page class. You can consume this only one time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageDisappearingAttribute : Attribute
    {
        public string PageDisappearingTaskName { get; } = string.Empty;

        public PageDisappearingAttribute(string pageDisappearingTaskName)
        {
            PageDisappearingTaskName = pageDisappearingTaskName;
        }
    }
}
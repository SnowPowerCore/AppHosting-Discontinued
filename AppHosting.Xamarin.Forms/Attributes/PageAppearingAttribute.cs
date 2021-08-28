using System;

namespace AppHosting.Xamarin.Forms.Attributes
{
    /// <summary>
    /// Please, use it on a page class. You can consume this only one time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageAppearingAttribute : Attribute
    {
        public string PageAppearingTaskName { get; } = string.Empty;

        public PageAppearingAttribute(string pageAppearingTaskName)
        {
            PageAppearingTaskName = pageAppearingTaskName;
        }
    }
}
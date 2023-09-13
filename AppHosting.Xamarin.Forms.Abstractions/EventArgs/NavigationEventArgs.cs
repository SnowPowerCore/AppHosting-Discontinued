using System;

namespace AppHosting.Xamarin.Forms.Abstractions.EventArgs
{
    public class NavigationEventArgs
    {
        public Type NavigationPageType { get; set; }

        public bool Cancel { get; set; } = false;
    }
}
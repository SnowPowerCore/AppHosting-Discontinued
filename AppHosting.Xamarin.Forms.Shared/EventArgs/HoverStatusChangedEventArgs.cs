using AppHosting.Xamarin.Forms.Shared.Enums;

namespace AppHosting.Xamarin.Forms.Shared.EventArgs
{
    public class HoverStatusChangedEventArgs : System.EventArgs
    {
        internal HoverStatusChangedEventArgs(HoverStatus status)
            => Status = status;

        public HoverStatus Status { get; }
    }
}
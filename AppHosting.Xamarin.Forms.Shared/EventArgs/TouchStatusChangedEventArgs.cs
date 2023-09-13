using AppHosting.Xamarin.Forms.Shared.Enums;

namespace AppHosting.Xamarin.Forms.Shared.EventArgs
{
    public class TouchStatusChangedEventArgs : System.EventArgs
    {
        internal TouchStatusChangedEventArgs(TouchStatus status)
            => Status = status;

        public TouchStatus Status { get; }
    }
}
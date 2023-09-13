using AppHosting.Xamarin.Forms.Shared.Enums;

namespace AppHosting.Xamarin.Forms.Shared.EventArgs
{
    public class TouchInteractionStatusChangedEventArgs : System.EventArgs
    {
        internal TouchInteractionStatusChangedEventArgs(TouchInteractionStatus touchInteractionStatus)
            => TouchInteractionStatus = touchInteractionStatus;

        public TouchInteractionStatus TouchInteractionStatus { get; }
    }
}
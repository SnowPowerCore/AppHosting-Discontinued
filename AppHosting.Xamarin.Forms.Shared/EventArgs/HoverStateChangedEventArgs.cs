using AppHosting.Xamarin.Forms.Shared.Enums;

namespace AppHosting.Xamarin.Forms.Shared.EventArgs
{
    public class HoverStateChangedEventArgs : System.EventArgs
    {
        internal HoverStateChangedEventArgs(HoverState state)
            => State = state;

        public HoverState State { get; }
    }
}
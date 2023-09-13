using AppHosting.Xamarin.Forms.Shared.Enums;

namespace AppHosting.Xamarin.Forms.Shared.EventArgs
{
    public class TouchStateChangedEventArgs : System.EventArgs
    {
        internal TouchStateChangedEventArgs(TouchState state)
            => State = state;

        public TouchState State { get; }
    }
}
using UIKit;

namespace AppHosting.Xamarin.Forms.iOS
{
    internal static partial class XCT
    {
        private static bool? _isiOS13OrNewer;

        internal static bool IsiOS13OrNewer => _isiOS13OrNewer == null
            ? (_isiOS13OrNewer = UIDevice.CurrentDevice.CheckSystemVersion(13, 0)).Value
            : _isiOS13OrNewer.Value;
    }
}
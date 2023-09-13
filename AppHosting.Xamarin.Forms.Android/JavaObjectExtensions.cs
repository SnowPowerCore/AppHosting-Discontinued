using System;

namespace AppHosting.Xamarin.Forms.Android
{
    internal static class JavaObjectExtensions
    {
        internal static bool IsDisposed(this Java.Lang.Object obj)
            => obj.Handle == IntPtr.Zero;

        internal static bool IsAlive(this Java.Lang.Object obj)
            => obj != null && !obj.IsDisposed();

        internal static bool IsDisposed(this global::Android.Runtime.IJavaObject obj)
            => obj.Handle == IntPtr.Zero;

        internal static bool IsAlive(this global::Android.Runtime.IJavaObject obj)
            => obj != null && !obj.IsDisposed();
    }
}
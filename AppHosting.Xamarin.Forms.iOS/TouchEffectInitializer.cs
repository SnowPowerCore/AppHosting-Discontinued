namespace AppHosting.Xamarin.Forms.iOS
{
    public sealed class TouchEffectInitializer
    {
        public static void Initialize() =>
            _ = new PlatformTouchEffect();
    }
}
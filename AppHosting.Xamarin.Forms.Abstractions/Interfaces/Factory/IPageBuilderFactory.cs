using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Factory
{
    public interface IPageBuilderFactory
    {
        IPageBuilder CreatePageBuilder();
    }
}
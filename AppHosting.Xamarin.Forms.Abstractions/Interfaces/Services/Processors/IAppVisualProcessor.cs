using AppHosting.Xamarin.Forms.Abstractions.Delegates;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors
{
    public interface IAppVisualProcessor
    {
        PageDelegate PageProcessing { get; }

        ElementDelegate ElementProcessing { get; }
    }
}
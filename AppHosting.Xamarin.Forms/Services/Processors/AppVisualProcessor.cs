using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Factory;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Services.Processors;
using System;

namespace AppHosting.Xamarin.Forms.Services.Processors
{
    public class AppVisualProcessor : IAppVisualProcessor
    {
        public PageDelegate PageProcessing { get; }

        public ElementDelegate ElementProcessing { get; }

        public AppVisualProcessor(IPageBuilderFactory pageBuilderFactory,
                                  IElementBuilderFactory elementBuilderFactory,
                                  IAppStartup startup)
        {
            var pageBuilder = pageBuilderFactory.CreatePageBuilder();
            var elementBuilder = elementBuilderFactory.CreateElementBuilder();

            Action<IPageBuilder> configurePage = startup.ConfigurePage;
            configurePage(pageBuilder);

            Action<IElementBuilder> configureElement = startup.ConfigureElement;
            configureElement(elementBuilder);

            PageProcessing = pageBuilder.Build();
            ElementProcessing = elementBuilder.Build();
        }
    }
}
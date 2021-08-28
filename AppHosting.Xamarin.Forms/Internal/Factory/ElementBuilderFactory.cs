using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Factory;
using AppHosting.Xamarin.Forms.Internal.Builders;
using System;

namespace AppHosting.Xamarin.Forms.Internal.Factory
{
    internal class ElementBuilderFactory : IElementBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ElementBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IElementBuilder CreateElementBuilder() =>
            new ElementBuilder(_serviceProvider);
    }
}
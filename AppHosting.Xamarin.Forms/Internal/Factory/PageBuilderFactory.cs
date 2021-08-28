using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Factory;
using AppHosting.Xamarin.Forms.Internal.Builders;
using System;

namespace AppHosting.Xamarin.Forms.Internal.Factory
{
    internal class PageBuilderFactory : IPageBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PageBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPageBuilder CreatePageBuilder() =>
            new PageBuilder(_serviceProvider);
    }
}
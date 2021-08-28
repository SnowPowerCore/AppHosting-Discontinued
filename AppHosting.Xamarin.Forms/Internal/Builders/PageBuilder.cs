using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Internal.Builders
{
    internal class PageBuilder : IPageBuilder
    {
        private const string ApplicationServicesKey = "application.Services";
        private readonly List<Func<PageDelegate, PageDelegate>> _components = new();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider ApplicationServices
        {
            get => GetProperty<IServiceProvider>(ApplicationServicesKey)!;
            set => SetProperty(ApplicationServicesKey, value);
        }

        public IDictionary<string, object> Properties { get; } =
            new Dictionary<string, object>();

        public PageBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        private PageBuilder(PageBuilder builder)
        {
            Properties = builder.Properties;
        }

        public PageDelegate Build()
        {
            PageDelegate p = page => Task.CompletedTask;

            for (var c = _components.Count - 1; c >= 0; c--)
                p = _components[c](p);

            return p;
        }

        public IPageBuilder New() =>
            new PageBuilder(this);

        public IPageBuilder Use(Func<PageDelegate, PageDelegate> step)
        {
            _components.Add(step);
            return this;
        }

        private T? GetProperty<T>(string key) =>
            Properties.TryGetValue(key, out var value) ? (T?)value : default;

        private void SetProperty<T>(string key, T value) =>
            Properties[key] = value;
    }
}
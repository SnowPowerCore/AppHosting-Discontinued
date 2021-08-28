using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppHosting.Xamarin.Forms.Internal.Builders
{
    internal class ElementBuilder : IElementBuilder
    {
        private const string ApplicationServicesKey = "application.Services";
        private readonly List<Func<ElementDelegate, ElementDelegate>> _components = new();

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

        public ElementBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        private ElementBuilder(ElementBuilder builder)
        {
            Properties = builder.Properties;
        }

        public ElementDelegate Build()
        {
            ElementDelegate e = element => Task.CompletedTask;

            for (var c = _components.Count - 1; c >= 0; c--)
                e = _components[c](e);

            return e;
        }

        public IElementBuilder New() =>
            new ElementBuilder(this);

        public IElementBuilder Use(Func<ElementDelegate, ElementDelegate> step)
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
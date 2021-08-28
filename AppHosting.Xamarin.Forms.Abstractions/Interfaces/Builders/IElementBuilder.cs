using AppHosting.Xamarin.Forms.Abstractions.Delegates;
using System;
using System.Collections.Generic;

namespace AppHosting.Xamarin.Forms.Abstractions.Interfaces.Builders
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure an application's request pipeline.
    /// </summary>
    public interface IElementBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between steps.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Adds a step delegate to the application's request pipeline.
        /// </summary>
        /// <param name="step">The "step" delegate.</param>
        /// <returns>The <see cref="IElementBuilder"/>.</returns>
        IElementBuilder Use(Func<ElementDelegate, ElementDelegate> step);

        /// <summary>
        /// Creates a new <see cref="IElementBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="IElementBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IElementBuilder"/>.</returns>
        IElementBuilder New();

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        ElementDelegate Build();
    }
}
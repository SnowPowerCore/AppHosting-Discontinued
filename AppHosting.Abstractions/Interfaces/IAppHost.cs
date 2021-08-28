using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppHosting.Abstractions.Interfaces
{
    /// <summary>
    /// Represents a configured mobile host.
    /// </summary>
    public interface IAppHost : IDisposable
    {
        /// <summary>
        /// The <see cref="IServiceProvider"/> for the mobile host.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Starts application synchronously.
        /// </summary>
        TApp Start<TApp>();

        /// <summary>
        /// Starts application asynchronously.
        /// </summary>
        /// <typeparam name="TApp">Registered mobile framework main application class</typeparam>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="IAppHost"/> starts.</returns>
        Task<TApp> StartAsync<TApp>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempt to gracefully stop the host.
        /// </summary>
        /// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="IAppHost"/> stops.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
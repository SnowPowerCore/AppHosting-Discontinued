using Microsoft.Extensions.Hosting;

namespace AppHosting.Abstractions.Interfaces
{
    /// <summary>
    /// Allows consumers to perform cleanup during application sleep/resume.
    /// </summary>
    public interface IAppHostLifetime : IHostApplicationLifetime
    {
        /// <summary>
        /// Triggered when the application has gone to sleep.
        /// </summary>
        ILifecycleRegister ApplicationSleeping { get; }

        /// <summary>
        /// Triggered when the application has resumed.
        /// </summary>
        ILifecycleRegister ApplicationResuming { get; }

        void NotifySleeping();

        void NotifyResuming();
    }
}
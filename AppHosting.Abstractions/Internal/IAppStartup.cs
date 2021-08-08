using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppHosting.Abstractions.Internal
{
    public interface IAppStartup
    {
        IServiceProvider ConfigureServices(IServiceCollection services);

        void RegisterRoutes();
    }
}
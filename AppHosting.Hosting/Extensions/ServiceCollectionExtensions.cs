using Microsoft.Extensions.DependencyInjection;

namespace AppHosting.Hosting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Clone(this IServiceCollection serviceCollection)
        {
            IServiceCollection clone = new ServiceCollection();
            foreach (var service in serviceCollection)
                clone.Add(service);
            return clone;
        }
    }
}
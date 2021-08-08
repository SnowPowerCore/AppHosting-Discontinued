using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace AppHosting.Hosting.Internal
{
    internal class AppHostOptions
    {
        public string ApplicationName { get; set; }

        public string Environment { get; set; }

        public string ContentRoot { get; set; }

        public AppHostOptions() { }

        public AppHostOptions(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            ApplicationName = configuration[HostDefaults.ApplicationKey]
                ?? typeof(AppHostOptions).Assembly.GetName().Name;
            Environment = configuration[HostDefaults.EnvironmentKey];
        }
    }
}
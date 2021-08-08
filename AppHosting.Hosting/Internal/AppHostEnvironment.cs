using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace AppHosting.Hosting.Internal
{
    internal class AppHostEnvironment : IHostEnvironment
    {
        public AppHostEnvironment() { }

        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; } =
            typeof(AppHostBuilder).Assembly?.GetName().Name;

        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();

        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
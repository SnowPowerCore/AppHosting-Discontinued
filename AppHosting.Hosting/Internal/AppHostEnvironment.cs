using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace AppHosting.Hosting.Internal
{
    internal class AppHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } =
            Environments.Production;

        public string ApplicationName { get; set; } =
            Assembly.GetEntryAssembly()?.GetName().Name;

        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();

        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
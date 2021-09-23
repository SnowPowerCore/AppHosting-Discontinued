using AppHosting.Hosting.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace AppHosting.Hosting.Extensions
{
    internal static class AppHostExtensions
    {
        internal static void Initialize(
            this IHostEnvironment hostingEnvironment, string contentRootPath, AppHostOptions options)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(contentRootPath))
            {
                throw new ArgumentException(
                    "A valid non-empty content root must be provided.",
                    nameof(contentRootPath));
            }

            if (!Directory.Exists(contentRootPath))
            {
                throw new ArgumentException(
                    $"The content root '{contentRootPath}' does not exist.",
                    nameof(contentRootPath));
            }

            hostingEnvironment.ApplicationName = options.ApplicationName;
            hostingEnvironment.ContentRootPath = contentRootPath;
            hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(
                hostingEnvironment.ContentRootPath);

            hostingEnvironment.EnvironmentName = options.Environment
                ?? hostingEnvironment.EnvironmentName;
        }
    }
}
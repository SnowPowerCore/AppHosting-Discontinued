using System;
using System.Linq;
using System.Reflection;

namespace AppHosting.Hosting.Internal
{
    internal class StartupLoader
    {
        public static Type FindStartupType(string startupAssemblyName, string environmentName)
        {
            if (string.IsNullOrEmpty(startupAssemblyName))
            {
                throw new ArgumentException(
                    string.Format("A startup method, startup type or startup assembly is required. If specifying an assembly, '{0}' cannot be default or empty.",
                    nameof(startupAssemblyName)),
                    nameof(startupAssemblyName));
            }

            var assembly = Assembly.Load(new AssemblyName(startupAssemblyName));
            if (assembly == default)
            {
                throw new InvalidOperationException(string.Format("The assembly '{0}' failed to load.", startupAssemblyName));
            }

            var startupNameWithEnv = "Startup" + environmentName;
            var startupNameWithoutEnv = "Startup";

            // Check the most likely places first
            var type =
                assembly.GetType(startupNameWithEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithEnv) ??
                assembly.GetType(startupNameWithoutEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithoutEnv);

            if (type == default)
            {
                // Full scan
                var definedTypes = assembly.DefinedTypes.ToList();

                var startupType1 = definedTypes.Where(info => info.Name.Equals(startupNameWithEnv, StringComparison.OrdinalIgnoreCase));
                var startupType2 = definedTypes.Where(info => info.Name.Equals(startupNameWithoutEnv, StringComparison.OrdinalIgnoreCase));

                var typeInfo = startupType1.Concat(startupType2).FirstOrDefault();
                if (typeInfo != default)
                {
                    type = typeInfo.AsType();
                }
            }

            if (type == default)
            {
                throw new InvalidOperationException(string.Format("A type named '{0}' or '{1}' could not be found in assembly '{2}'.",
                    startupNameWithEnv,
                    startupNameWithoutEnv,
                    startupAssemblyName));
            }

            return type;
        }
    }
}
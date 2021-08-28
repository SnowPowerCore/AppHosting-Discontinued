using System.Diagnostics;

namespace AppHosting.Hosting.Internal
{
    public class AppRuntimeConfiguration
    {
        private static bool _debugMode;

        private static bool IsDebugMode()
        {
            SetDebugMode();
            return _debugMode;
        }

        //This method will be loaded only in the case of DEBUG mode. 
        //In RELEASE mode, all the calls to this method will be ignored by runtime.
        [Conditional("DEBUG")]
        private static void SetDebugMode()
        {
            _debugMode = true;
        }

        public static string CompilationMode => IsDebugMode()
            ? "Debug"
            : "Release";
    }
}
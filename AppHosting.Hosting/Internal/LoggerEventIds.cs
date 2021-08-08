namespace AppHosting.Hosting.Internal
{
    internal static class LoggerEventIds
    {
        public const int ApplicationStarting = 3;
        public const int ApplicationStarted = 4;
        public const int ApplicationShutdown = 5;
        public const int ApplicationStartupException = 6;
        public const int ApplicationStoppingException = 7;
        public const int ApplicationStoppedException = 8;
        public const int ApplicationSleeping = 9;
        public const int ApplicationResuming = 10;
        public const int ApplicationSleepingException = 11;
        public const int ApplicationResumingException = 12;
        public const int HostingStartupAssemblyException = 13;
    }
}
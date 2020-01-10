using Microsoft.Extensions.Configuration;
using Serilog;

namespace Xerris.DotNet.Core.Logging
{
    public static class LogStartup
    {
        private static readonly object Mutex = new object();
        private static bool initialized;

        public static void Initialize(IConfiguration configuration)
        {
            if (initialized) return;
            lock (Mutex)
            {
                if (initialized) return;
                
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.Console(outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                initialized = true;
            }
        }
    }
}
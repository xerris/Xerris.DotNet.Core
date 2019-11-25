using Microsoft.Extensions.Configuration;
using Serilog;

namespace Xerris.DotNet.Core.Core.Logging
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
                    .CreateLogger();

                initialized = true;
            }
        }
    }
}
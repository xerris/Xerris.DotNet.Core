using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

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

                var hasSerilog = configuration.GetSection("Serilog") != null;

                if (hasSerilog)
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console(outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();
                }
                

                initialized = true;
            }
        }
    }
}
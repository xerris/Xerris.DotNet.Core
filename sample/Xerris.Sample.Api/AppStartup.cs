using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xerris.DotNet.Core;

namespace Xerris.Sample.Api
{
    public class AppStartUp : IAppStartup
    {
        public IConfiguration StartUp(IServiceCollection collection)
        {
            var builder = new ApplicationConfigurationBuilder<ApplicationConfig>();
            var appConfig = builder.Build();
            
            collection.AddSingleton<IApplicationConfig>(appConfig);
            collection.AutoRegister(GetType().Assembly);
            
            return builder.Configuration;;
        }

        public void InitializeLogging(IConfiguration configuration, Action<IConfiguration> defaultConfig)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
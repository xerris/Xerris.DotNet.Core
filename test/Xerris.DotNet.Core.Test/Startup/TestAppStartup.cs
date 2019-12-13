using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Xerris.DotNet.Core.Test.Startup
{
    public class TestAppStartup : IAppStartup
    {
        public IConfiguration StartUp(IServiceCollection collection)
        {
            var builder = new ApplicationConfigurationBuilder<ApplicationConfig>(collection);
            var appConfig = builder.Build();
            collection.AddSingleton<IApplicationConfig>(appConfig);
            return builder.Configuration;
        }
    }
}
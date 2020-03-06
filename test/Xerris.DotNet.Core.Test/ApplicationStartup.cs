using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.TestSupport;

namespace Xerris.DotNet.Core.Test
{
    public class ApplicationStartup : IAppStartup
    {
        public IConfiguration StartUp(IServiceCollection collection)
        {
            var builder = new ApplicationConfigurationBuilder<ApplicationConfig>(collection);
            var appConfig = builder.Build();

            collection.AddSingleton<IApplicationConfig>(appConfig);
            collection.AddSingleton<IAddMe, AddMe>();

            collection.AutoRegister(GetType().Assembly).AutoRegister(typeof(IAddMe).Assembly);
            return builder.Configuration;
        }
    }
}
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Xerris.DotNet.Core
{
    public interface IAppStartup
    {
        IConfiguration StartUp(IServiceCollection collection);
        void InitializeLogging(IConfiguration configuration, Action<IConfiguration> defaultConfig);
    }
}
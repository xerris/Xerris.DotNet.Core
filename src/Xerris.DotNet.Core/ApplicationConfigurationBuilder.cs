using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Xerris.DotNet.Core
{
    public class ApplicationConfigurationBuilder<T> where T : IApplicationConfigBase, new()
    {
        private IConfiguration configuration;
        private readonly IServiceCollection collection;

        public ApplicationConfigurationBuilder(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public T Build()
        {
            var appConfig = new T();
            Configuration.Bind(appConfig);
            appConfig.AwsOptions = configuration.GetAWSOptions();
            return appConfig;
        }

        public IConfiguration Configuration
        {
            get
            {
                var stageName = Environment.GetEnvironmentVariable("stageName");
                return configuration ??= new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", false)
                            .AddJsonFile($"appsettings.{stageName}.json", true)
                            .AddEnvironmentVariables()
                            .Build();
            }
        }
    }
}
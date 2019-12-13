using Amazon.Extensions.NETCore.Setup;

namespace Xerris.DotNet.Core.Test.Startup
{
    public interface IApplicationConfig : IApplicationConfigBase
    {
        string AllowedHosts { get; }
    }

    public class ApplicationConfig : IApplicationConfig
    {
        public AWSOptions AwsOptions { get; set; }
        public string AllowedHosts { get; set; }
    }
}
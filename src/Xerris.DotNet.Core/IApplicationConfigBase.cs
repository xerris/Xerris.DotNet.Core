using Amazon.Extensions.NETCore.Setup;

namespace Xerris.DotNet.Core
{
    public interface IApplicationConfigBase
    {
        AWSOptions AwsOptions { get; set; }
    }
}
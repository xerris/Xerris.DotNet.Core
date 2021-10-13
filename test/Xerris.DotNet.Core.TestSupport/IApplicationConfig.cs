namespace Xerris.DotNet.Core.TestSupport
{
    public interface IApplicationConfig : IApplicationConfigBase
    {
        string AllowedHosts { get; }
        string ConnectionString { get; }
    }

    public class ApplicationConfig : IApplicationConfig
    {
        public string AllowedHosts { get; set; }
        public string ConnectionString { get; set; }
        
    }
}
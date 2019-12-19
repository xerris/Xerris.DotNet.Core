using FluentAssertions;
using Xerris.DotNet.Core.TestSupport;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Startup
{
    public class IoCTests
    {
        [Fact]
        public void ApplicationConfig()
        {
            var appConfig = IoC.Resolve<IApplicationConfig>();

            Validate.Begin()
                .IsNotNull(appConfig, "has an app config")
                .Check()
                .IsEqual(appConfig.AllowedHosts, "*", "got allowedHosts")
                .Check();
        }

        [Fact]
        public void MyService()
        {
            var service = IoC.Resolve<IService>();
            service.Should().NotBeNull();
        }
        

    }
}
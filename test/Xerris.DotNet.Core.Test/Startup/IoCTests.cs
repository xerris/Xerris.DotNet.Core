using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        public void UsingAnotherServiceCollection()
        {
            var privateCollection = new ServiceCollection();
            IoC.Initialize(privateCollection);

            IoC.Resolve<IApplicationConfig>().Should().BeOfType<ApplicationConfig>();
            IoC.Resolve<IService>().Should().BeOfType<MyService>();
        }

        [Fact]
        public void MyService()
        {
            var service = IoC.Resolve<IService>();
            service.Should().NotBeNull();
        }
    }
}
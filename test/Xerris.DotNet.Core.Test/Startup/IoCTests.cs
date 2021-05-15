using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.TestSupport;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Startup
{
    public class IoCTests : IDisposable
    {
        public IoCTests()
        {
            Environment.SetEnvironmentVariable(nameof(IApplicationConfig.ConnectionString), "connectme");
        }

        [Fact]
        public void ApplicationConfig()
        {
 
            var appConfig = IoC.Resolve<IApplicationConfig>();

            Validate.Begin()
                .IsNotNull(appConfig, "has an app config")
                .Check()
                .IsEqual(appConfig.AllowedHosts, "*", "got allowedHosts")
                .IsEqual(appConfig.ConnectionString, "connectme", nameof(IApplicationConfig.ConnectionString))
                .Check();
        }

        [Fact]
        public void UsingAnotherServiceCollection()
        {
            var privateCollection = new ServiceCollection();
            IoC.ConfigureServiceCollection(privateCollection);

            IoC.Resolve<IApplicationConfig>().Should().BeOfType<ApplicationConfig>();
            IoC.Resolve<IService>().Should().BeOfType<MyService>();
            IoC.Resolve<IAddMe>().Should().BeOfType<AddMe>();
        }

        [Fact]
        public void MyService()
        {
            var service = IoC.Resolve<IService>();
            service.Should().NotBeNull();
            IoC.Resolve<IAddMe>().Should().BeOfType<AddMe>();
        }

        [Fact]
        public void ResolveShouldReturnTheSameInstance()
        {
            var service = IoC.Resolve<IService>();
            IoC.Resolve<IService>().Should().BeSameAs(service);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(nameof(IApplicationConfig.ConnectionString), null);
        }
    }
}
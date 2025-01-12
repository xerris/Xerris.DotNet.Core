namespace Xerris.DotNet.Core.Test.DI
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using FluentAssertions;

    public class IoCTests
    {
        public IoCTests()
        {
            IoC.Reset();
        }

        [Fact]
        public void ConfigureServiceCollection_ShouldSetServiceCollectionProvider()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            IoC.ConfigureServiceCollection(services);

            // Assert
            var serviceProvider = IoC.CreateScope().ServiceProvider;
            serviceProvider.Should().NotBeNull();
        }

        [Fact]
        public void Resolve_ShouldReturnRegisteredService()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IService, ServiceImplementation>();
            IoC.Reset(services);

            // Act
            var service = IoC.Resolve<IService>();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ServiceImplementation>();
        }

        [Fact]
        public void TryResolve_ShouldReturnRegisteredService()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IService, ServiceImplementation>();
            IoC.Reset(services);

            // Act
            var service = IoC.TryResolve<IService, DefaultServiceImplementation>();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ServiceImplementation>();
        }

        [Fact]
        public void TryResolve_ShouldReturnDefaultService_WhenNotRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            IoC.Reset(services);

            // Act
            var service = IoC.TryResolve<IService, DefaultServiceImplementation>();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<DefaultServiceImplementation>();
        }

        [Fact]
        public void CreateScope_ShouldReturnNewServiceScope()
        {
            // Arrange
            var services = new ServiceCollection();
            IoC.Reset(services);

            // Act
            using (var scope = IoC.CreateScope())
            {
                // Assert
                scope.Should().NotBeNull();
                scope.ServiceProvider.Should().NotBeNull();
            }
        }
    }
}
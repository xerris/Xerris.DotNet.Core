using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.DI;
using Xunit;

namespace Xerris.DotNet.Core.Test.DI
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDefaultSingleton_ShouldAddService_WhenNotAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDefaultSingleton<IService, ServiceImplementation>();

            // Assert
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IService>();
            service.Should().NotBeNull();
            service.Should().BeOfType<ServiceImplementation>();
        }

        [Fact]
        public void AddDefaultScoped_ShouldAddService_WhenNotAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDefaultScoped<IService, ServiceImplementation>();

            // Assert
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var service = scope.ServiceProvider.GetService<IService>();
            service.Should().NotBeNull();
            service.Should().BeOfType<ServiceImplementation>();
        }

        [Fact]
        public void AddDefaultTransient_ShouldAddService_WhenNotAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDefaultTransient<IService, ServiceImplementation>();

            // Assert
            var provider = services.BuildServiceProvider();
            var service1 = provider.GetService<IService>();
            var service2 = provider.GetService<IService>();
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().BeOfType<ServiceImplementation>();
            service2.Should().BeOfType<ServiceImplementation>();
            service1.Should().NotBeSameAs(service2);
        }
        
        [Fact]
        public void ReplaceSingleton_ShouldReplaceService_WhenAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IService, OriginalServiceImplementation>();

            // Act
            services.ReplaceSingleton<IService, NewServiceImplementation>();

            // Assert
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IService>();
            service.Should().NotBeNull();
            service.Should().BeOfType<NewServiceImplementation>();
        }

        [Fact]
        public void ReplaceScoped_ShouldReplaceService_WhenAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<IService, OriginalServiceImplementation>();

            // Act
            services.ReplaceScoped<IService, NewServiceImplementation>();

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            
            // Assert
            var service = scope.ServiceProvider.GetService<IService>();
            service.Should().NotBeNull();
            service.Should().BeOfType<NewServiceImplementation>();
        }

        [Fact]
        public void ReplaceTransient_ShouldReplaceService_WhenAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IService, OriginalServiceImplementation>();

            // Act
            services.ReplaceTransient<IService, NewServiceImplementation>();

            // Assert
            var provider = services.BuildServiceProvider();
            var service1 = provider.GetService<IService>();
            var service2 = provider.GetService<IService>();
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().BeOfType<NewServiceImplementation>();
            service2.Should().BeOfType<NewServiceImplementation>();
            service1.Should().NotBeSameAs(service2);
        }        
    }
}
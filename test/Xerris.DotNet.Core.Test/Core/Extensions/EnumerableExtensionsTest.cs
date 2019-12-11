using FluentAssertions;
using Xerris.DotNet.Core.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void GetEmail()
        {
            new[] {"dxpw-admin", "nutrien.user@nutrien.com"}
                .GetEmail().Should().Be("nutrien.user@nutrien.com");
            
            new[] {"testuser22@nutrien.com", "dwpw-admin"}
                .GetEmail().Should().Be("testuser22@nutrien.com");
            
            new[] {"dxpw-admin", "Joan.Lynn.External@nutrien.com"}
                .GetEmail().Should().Be("Joan.Lynn.External@nutrien.com");
            
            new[] {"dxpw-admin", "stagedaniel@gmail.test"}
                .GetEmail().Should().Be("stagedaniel@gmail.test");
            
            new[] {"dxpw-admin", "newemail@gmail.om"}
                .GetEmail().Should().Be("newemail@gmail.om");
        }

        [Fact]
        public void NoEmail()
        {
            
        }
    }
}
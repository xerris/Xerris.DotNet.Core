using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class NumericExtensionsTest
    {
        [Fact]
        public void DoubleRoundedTo() => double.Parse("6.01").RoundedTo(2).Should().Be(6.01);
    }
}
using System;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void ToDate()
        {
            const string dateString = "20201221";
            dateString.ToDate("yyyyMMdd")
                      .Should().Be(new DateTime(2020, 12, 21));
        }

        [Theory]
        [InlineData("yyyy-MM-dd", "2022-01-30")]
        [InlineData("yyyyMMdd", "20220130")]
        [InlineData("s", "2022-01-30T00:00:00")]
        public void Formatted(string format, string expected)
        {
            var date = new DateTime(2022, 01, 30);
            date.Formatted(format).Should().Be(expected);
        }
        
        [Fact]
        public void ToDateBadDateString()
        {
            const string dateString = "20201221x";

            Assert.Throws<FormatException>(() => dateString.ToDate("yyyyMMdd"));
        }
    }
}
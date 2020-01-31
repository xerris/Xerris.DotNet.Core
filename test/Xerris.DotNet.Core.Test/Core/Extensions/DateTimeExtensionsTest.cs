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
        [Fact]
        public void ToDateBadDateString()
        {
            const string dateString = "20201221x";

            Assert.Throws<FormatException>(() => dateString.ToDate("yyyyMMdd"));
        }
    }
}
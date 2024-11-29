using System;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class DateTimeExtensionsTest
    {
        private static readonly DateTime UnixStartTime = DateTime.UnixEpoch;
        private readonly ITestOutputHelper outputHelper;

        public DateTimeExtensionsTest(ITestOutputHelper outputHelper)
            => this.outputHelper = outputHelper;


        [Fact]
        public void Earliest_Midnight() => DateTime.Today.Earliest().Should().Be(DateTime.Today);

        [Fact]
        public void Earliest_At00_00_01() => DateTime.Today.AddSeconds(1).Earliest().Should().Be(DateTime.Today);

        [Fact]
        public void Earliest_At00_01_00() => DateTime.Today.AddMinutes(1).Earliest().Should().Be(DateTime.Today);

        [Fact]
        public void Earliest_At01_00_00() => DateTime.Today.AddHours(1).Earliest().Should().Be(DateTime.Today);
        
        [Fact]
        public void ShouldConvertFromDateTimeToUnixTimeStamp()
        {
            var dateUtc = new DateTime(2021, 03, 9, 15, 41, 44, 123, DateTimeKind.Utc);
            dateUtc.ConvertToUnixTimeStamp().Should().Be(1615304504123);

            var dateLocal = new DateTime(2021, 03, 9, 15, 41, 44, DateTimeKind.Utc); //should treat as utc
            dateLocal.ConvertToUnixTimeStamp().Should().Be(1615304504000);

            UnixStartTime.ConvertToUnixTimeStamp().Should().Be(0);
        }

        [Fact]
        public void ShouldConvertFromUnixTimeStamp()
        {
            const double timestamp1 = 1385701110123;
            timestamp1.ConvertFromUnixTimeStamp().Should()
                .BeSameDateAs(new DateTime(2013, 11, 29, 16, 58, 30, 123, DateTimeKind.Utc));

            const double timestamp2 = 1615304342456;
            timestamp2.ConvertFromUnixTimeStamp().Should()
                .BeSameDateAs(new DateTime(2021, 03, 9, 15, 39, 2, 456, DateTimeKind.Utc));

            const double timestamp3 = 1616701401000;
            timestamp3.ConvertFromUnixTimeStamp().Should()
                .BeSameDateAs(new DateTime(2021, 03, 25, 7, 44, 00, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldConvertBackAndForth()
        {
            const double timeStamp = 1616686700;
            var date = timeStamp.ConvertFromUnixTimeStamp();
            date.ConvertToUnixTimeStamp().Should().Be(timeStamp);
        }

        [Fact]
        public void OneSecondDifferenceShouldCreateDifferentTimeStamps()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 23, 14, 00, DateTimeKind.Utc);
            var dateTime2 = new DateTime(2021, 03, 15, 23, 14, 01, DateTimeKind.Utc);

            var dt1 = dateTime1.ConvertToUnixTimeStamp();
            var dt2 = dateTime2.ConvertToUnixTimeStamp();
            dt1.Should().NotBe(dt2);
        }

        [Fact]
        public void ShouldReduceMillisecondPrecision()
        {
            const string fullTimeFormat = "HH:mm:ss:ffffff";

            var now = DateTime.UtcNow;
            var reduced = now.ReduceMillisecondPrecision();

            outputHelper.WriteLine("now:               {0}", now.ToString(fullTimeFormat));
            outputHelper.WriteLine("reduced precision: {0}", reduced.ToString(fullTimeFormat));

            reduced.ToString(fullTimeFormat).Should().BeEquivalentTo(reduced.ToString("HH:mm:ss:fff000"));
            reduced.ToString(fullTimeFormat).Should()
                .NotBe(reduced.ToString("HH:mm:ss:000000")); //ensure all milliseconds are not truncated
        }

        [Fact]
        public void ShouldTruncateMilliseconds()
        {
            var toTruncate = new DateTime(2000, 01, 11, 10, 30, 12, DateTimeKind.Utc);
            toTruncate = toTruncate.AddMilliseconds(5).UtcTruncateMilliseconds();

            toTruncate.Should().HaveYear(2000);
            toTruncate.Should().HaveMonth(01);
            toTruncate.Should().HaveDay(11);
            toTruncate.Should().HaveHour(10);
            toTruncate.Should().HaveMinute(30);
            toTruncate.Should().HaveSecond(12);
            toTruncate.Millisecond.Should().Be(0);
            toTruncate.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void ShouldTruncateSeconds()
        {
            var toTruncate = new DateTime(2000, 01, 11, 10, 30, 12, DateTimeKind.Utc);
            toTruncate = toTruncate.AddMilliseconds(5).UtcTruncateSeconds();

            toTruncate.Should().HaveYear(2000);
            toTruncate.Should().HaveMonth(01);
            toTruncate.Should().HaveDay(11);
            toTruncate.Should().HaveHour(10);
            toTruncate.Should().HaveMinute(30);
            toTruncate.Should().HaveSecond(0);
            toTruncate.Millisecond.Should().Be(0);
            toTruncate.Kind.Should().Be(DateTimeKind.Utc);
        }

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
        public void ToDateBadDateString() => Assert.Throws<FormatException>(() => "20201221x".ToDate("yyyyMMdd"));
    }
}
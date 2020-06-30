using System;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Utilities;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    public class UtcTimezoneClockTest
    {
        private readonly TimeZoneClock utc;

        public UtcTimezoneClockTest()
        {
            utc = new TimeZoneClock(TimeZoneInfo.Utc);
        }

        [Fact]
        public void TodayAtPacific() => TestTodayAt(TimezoneOffset.Pacific);
        
        [Fact]
        public void TodayAtMountain() => TestTodayAt(TimezoneOffset.Mountain);
        
        [Fact]
        public void TodayAtCentral() => TestTodayAt(TimezoneOffset.Central);
        
        [Fact]
        public void TodayAtEastern() => TestTodayAt(TimezoneOffset.Eastern);

        [Theory]
        [InlineData("2020-05-01 05:00:00", "2020-04-30 23:00:00")] // Normal -7
        [InlineData("2020-05-01 00:00:00", "2020-04-30 18:00:00")] // Normal -7
        [InlineData("2020-05-01 12:00:00", "2020-05-01 6:00:00")]  // Normal -7
        [InlineData("2020-11-15 05:00:00", "2020-11-14 22:00:00")] // Daylight savings -6
        [InlineData("2020-11-15 00:00:00", "2020-11-14 17:00:00")] // Daylight savings -6
        [InlineData("2020-03-08 09:00:00", "2020-03-08 03:00:00")] // Daylight savings -6 (over Daylight savings date)
        [InlineData("2020-03-08 08:00:00", "2020-03-08 01:00:00")] // Normal -7 (over Daylight savings date)
        public void UtcToMountain(string actualUtcTime, string expectedLocalTime)
        {
            var localTime = DateTime.Parse(expectedLocalTime);
            var utcTime = DateTime.Parse(actualUtcTime);
            utc.Freeze(utcTime); 
            var dateTime = utc.NowAt(TimezoneOffset.Mountain);
            dateTime.Should().Be(localTime);
            utc.Thaw();
        }
        
        private void TestTodayAt(TimezoneOffset offset)
        {
            var todayUtc = CalculateTodayForOffset(offset);
            utc.TodayAt(offset).Should().Be(todayUtc);
        }

        private DateTime CalculateTodayForOffset(TimezoneOffset offset)
        {
            var gap = Math.Abs(offset.Offset);
            var toAdd = utc.Now.Hour - gap <= gap ? -1 : 0; //depending on time of day figure out what the expected 'today' is
            return  utc.Today.AddHours(toAdd).Earliest();
        }
    }
}
using System;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
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

        private void TestTodayAt(TimezoneOffset offset)
        {
            var todayUtc = utc.Today;
            utc.TodayAt(offset).Should().Be(todayUtc);
        }
    }
}
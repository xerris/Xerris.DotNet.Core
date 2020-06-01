using System;
using System.Runtime.InteropServices;
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
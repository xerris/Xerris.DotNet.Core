using System;
using Xerris.DotNet.Core.Time;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    public class TimezoneOffsetTest
    {
        private readonly TimeZoneClock utc;

        public TimezoneOffsetTest()
        {
            utc = new TimeZoneClock(TimeZoneInfo.Utc);
        }
        
        [Fact]
        public void TodayAtPacific() => TimezoneOffset.Pacific.TodayFrom(utc.Today);
        [Fact]
        public void PacificTodayFromUtc() => TimezoneOffset.Pacific.TodayFromUtc();
        
        [Fact]
        public void TodayAtMountain() => TimezoneOffset.Mountain.TodayFrom(utc.Today);
        [Fact]
        public void MountainTodayFromUtc() => TimezoneOffset.Mountain.TodayFromUtc();

        [Fact]
        public void TodayAtCentral() => TimezoneOffset.Central.TodayFrom(utc.Today);
        [Fact]
        public void CentralTodayFromUtc() => TimezoneOffset.Central.TodayFromUtc();

        [Fact]
        public void TodayAtEastern() => TimezoneOffset.Eastern.TodayFrom(utc.Today);
        [Fact]
        public void EasternFromUtc() => TimezoneOffset.Eastern.TodayFromUtc();
        
    }
}
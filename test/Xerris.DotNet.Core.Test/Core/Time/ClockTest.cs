using System;
using System.Threading;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Time;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    [Collection("base")]
    public class ClockTest : IDisposable
    {
        [Fact]
        public void CanGetEndOfTime()
        {
            Clock.EndOfTime.Should().Be(new DateTime(9999, 12, 31, 23, 59, 59,999));
        }

        [Fact]
        public void CanGetToday()
        {
            var today = Clock.Local.Today;
            today.Should().Be(DateTime.Today);
            today.Hour.Should().Be(0);
            today.Minute.Should().Be(0);
            today.Second.Should().Be(0);
            today.Millisecond.Should().Be(0);
        }

        [Fact]
        public void CanFreezeTime()
        {
            Clock.Local.Freeze();
            var frozen = Clock.Local.Now;
            Thread.Sleep(500); //wait 1/2 second;
            Clock.Local.Now.Should().Be(frozen);

            Clock.Local.Thaw();
            Thread.Sleep(500);

            Clock.Local.Now.IsCloseEnough(frozen, 100);
        }

        [Fact]
        public void CanGetMountainTime()
        {
            var now = Clock.MountainTime.Now;
        }
        
        [Fact]
        public void CanGetCentralTime()
        {
            var now = Clock.CentralTime.Now;
        }

        [Theory]
        [InlineData("2021-11-03 13:00:00", "2021-11-04 00:00:00")] // 1:00PM UTC is the next day in AEDT
        [InlineData("2021-11-03 12:59:00", "2021-11-03 00:00:00")] // before 1:00ON UTC is the same day in AEDT
        [InlineData("2021-09-03 14:00:00", "2021-09-04 00:00:00")] // 2:00PM UTC is the next day in AEST
        [InlineData("2021-09-03 13:59:00", "2021-09-03 00:00:00")] // before 2:00ON UTC is the same day in AEST
        public void CanGetTodayInAustralianEasternTime(string utcTimeAsString, string expectedAestDateAsString)
        {
            Clock.Utc.Freeze(DateTime.Parse(utcTimeAsString));
            Clock.AustralianEasternTime.Today.Should().Be(DateTime.Parse(expectedAestDateAsString));
        }

        public void Dispose()
        {
            Clock.Local.Thaw(); 
        }
    }
}
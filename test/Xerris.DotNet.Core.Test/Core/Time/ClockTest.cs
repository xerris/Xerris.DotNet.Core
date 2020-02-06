using System;
using System.Threading;
using FluentAssertions;
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
            Clock.Local.Now.Should().NotBe(frozen);
        }

        public void Dispose()
        {
            Clock.Local.Thaw(); 
        }
    }
}
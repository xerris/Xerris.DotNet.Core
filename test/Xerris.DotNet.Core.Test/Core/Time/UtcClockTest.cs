using System;
using System.Threading;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    public class UtcClockTest : IDisposable
    {
        public void Dispose()
        {
            UtcClock.Thaw();
        }


        [Fact]
        public void CanFreeze()
        {
            var now = new DateTime(2007, 01, 07, 14, 15, 0).ToUniversalTime();
            UtcClock.Freeze(now);
            UtcClock.Now.Should().Be(now);
        }

        [Fact]
        public void CanOnlyFreezeUtcTimes()
        {
            FluentActions.Invoking(() => UtcClock.Freeze(new DateTime(2007, 01, 07, 14, 15, 0)))
                .Should().Throw<Exception>()
                .WithMessage("Only UTC times are valid to be frozen");
        }

        [Fact]
        public void Now()
        {
            var oldNow = UtcClock.Now;
            Thread.Sleep(1);
            UtcClock.Now.Should().NotBe(oldNow);
        }

        [Fact]
        public void Freeze()
        {
            UtcClock.Freeze();
            var frozenNow = UtcClock.Now;
            Thread.Sleep(1);
            UtcClock.Now.Should().Be(frozenNow);
        }

        [Fact]
        public void Thaw()
        {
            UtcClock.Freeze();
            var wasFrozen = UtcClock.Now;
            Thread.Sleep(1);
            UtcClock.Thaw();
            Thread.Sleep(1);
            UtcClock.Now.Should().NotBe(wasFrozen);
        }
    }
}
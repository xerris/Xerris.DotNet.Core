using System;
using System.Threading;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Time;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    [Collection("base")]
    public class FreezeClockTest : IDisposable
    {
        public FreezeClockTest()
        {
            Clock.Local.Thaw();
            Clock.Utc.Thaw();
        }

        public void Dispose()
        {
            Clock.Local.Thaw();
            Clock.Utc.Thaw();
        }

        [Fact]
        public void CanFreezeClockUtc()
        {
            DateTime then;
            using (new FreezeClock())
            {
                then = Clock.Utc.Now;
                Thread.Sleep(500);
                Clock.Utc.Now.IsCloseEnough(then, .0001m);
            }

            Thread.Sleep(500);
            Clock.Utc.Now.Should().NotBe(then);
        }

        [Fact]
        public void CanFreezeClock()
        {
            DateTime then;
            using (new FreezeClock())
            {
                then = Clock.Local.Now;
                Thread.Sleep(500);
                Clock.Local.Now.IsCloseEnough(then, .0001m);
            }

            Thread.Sleep(500);
            Clock.Local.Now.Should().NotBe(then);
        }

        [Fact]
        public void CanFreezeClockForSpecificDateTime()
        {
            var then = new DateTime(2018, 01, 15);
            using (new FreezeClock(then))
            {
                Clock.Local.Now.IsCloseEnough(then, .0001m);
                Thread.Sleep(500);
                Clock.Local.Now.IsCloseEnough(then, .0001m);
            }

            Thread.Sleep(500);
            Clock.Local.Now.IsCloseEnough(then, .0001m);
        }

        [Fact]
        public void CanFreezeClockForSpecificDateTimeUtc()
        {
            var then = new DateTime(2018, 01, 15);
            using (new FreezeClock(then))
            {
                Clock.Utc.Now.IsCloseEnough(then, .0001m);
                Thread.Sleep(500);
                Clock.Utc.Now.IsCloseEnough(then, .0001m);
            }

            Thread.Sleep(500);
            Clock.Utc.Now.IsCloseEnough(then, .0001m);
        }
    }
}
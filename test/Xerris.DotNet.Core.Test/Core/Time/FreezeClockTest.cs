using System;
using System.Threading;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Time
{
    [Collection("base")]
    public class FreezeClockTest
    {
        public FreezeClockTest()
        {
            Clock.Local.Thaw();
        }

        [Fact]
        public void CanFreezeClock()
        {
            DateTime then;
            using (new FreezeClock())
            {
                then = Clock.Local.Now;
                Thread.Sleep(500);
                Clock.Local.Now.Should().Be(then);
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
                Clock.Local.Now.Should().Be(then);
                Thread.Sleep(500);
                Clock.Local.Now.Should().Be(then);
            }
            Thread.Sleep(500);
            Clock.Local.Now.Should().NotBe(then);
            
        }
    }
}
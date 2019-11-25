using System;
using System.Threading;
using FluentAssertions;
using Xerris.DotNet.Core.Core;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core
{
    [Collection("base")]
    public class FreezeClockTest
    {
        [Fact]
        public void CanFreezeClodk()
        {
            var then = Clock.Local.Now;
            using (var frozen = new FreezeClock())
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
            using (var frozen = new FreezeClock(then))
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
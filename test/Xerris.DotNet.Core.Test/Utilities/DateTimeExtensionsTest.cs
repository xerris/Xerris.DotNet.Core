using System;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Utilities;
using Xunit;

namespace Xerris.DotNet.Core.Test.Utilities
{
    public class DateTimeExtensionsTest
    {
        [Fact] 
        public void Earliest_Midnight() => DateTime.Today.Earliest().Should().Be(DateTime.Today);
        
        [Fact] 
        public void Earliest_At00_00_01() => DateTime.Today.AddSeconds(1).Earliest().Should().Be(DateTime.Today);
       
        [Fact] 
        public void Earliest_At00_01_00() => DateTime.Today.AddMinutes(1).Earliest().Should().Be(DateTime.Today);
        
        [Fact]
        public void Earliest_At01_00_00() => DateTime.Today.AddHours(1).Earliest().Should().Be(DateTime.Today);
        
    }
}
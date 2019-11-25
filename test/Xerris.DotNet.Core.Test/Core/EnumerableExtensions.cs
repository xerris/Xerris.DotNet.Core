using System;
using System.Collections.Generic;
using FluentAssertions;
using Xerris.DotNet.Core.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core
{
    public class EnumerableExtensions
    {
        [Fact]
        public void ForEach()
        {
            var count = 0;
                 
            new List<Action> { () => { count += 1; }, () => { count += 1; }, () => { count += 1; }}
                .ForEach(each => each());
            count.Should().Be(3);
        }

        [Fact]
        public void IsNullOrEmpty_Empty()
        {
            var items = new List<string> {};
            items.IsNullOrEmpty().Should().Be(true);
            items.IsNotNullOrEmpty().Should().Be(false);
        }

        [Fact]
        public void IsNullOrEmpty_null()
        {
            List<string> nullItems = null;
            nullItems.IsNullOrEmpty().Should().Be(true);
            nullItems.IsNotNullOrEmpty().Should().Be(false);
        }

        [Fact]
        public void BuildCsvString()
        {
            new[] {"hi", "there", "Xerris"}.BuildCsvString().Should().Be("hi, there, Xerris");
        }

        [Fact]
        public void BuildDeliminatedString()
        {
            new[] {"hi", "there", "Xerris"}.BuildDeliminatedString("-")
                .Should().Be("hi- there- Xerris");
        }
        [Fact]
        public void BuildDeliminatedQuotedString()
        {
            new[] {"hi", "there", "Xerris"}.BuildDeliminatedQuotedString("-")
                .Should().Be("'hi'- 'there'- 'Xerris'");
        }
    }
}
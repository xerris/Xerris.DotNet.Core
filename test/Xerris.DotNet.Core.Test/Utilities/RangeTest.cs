using System;
using System.Collections.Generic;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Utilities;
using Xunit;

namespace Xerris.DotNet.Core.Test.Utilities
{
    public class RangeTest
    {
        [Fact]
        public void ShouldInclude()
        {
            var range = new Range<int>(1, 10);

            for (var i = range.Start; i <= range.End; i++)
                range.Includes(i).Should().BeTrue();
        }

        [Fact]
        public void ShouldIncludeRange()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(1), today.AddDays(5));
            range.Includes(otherRange).Should().BeTrue();
        }

        [Fact]
        public void ShouldNotIncludeRange()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(-1), today.AddDays(5));
            range.Includes(otherRange).Should().BeFalse();
        }

        [Fact]
        public void ShouldOverlapRangeWhenOtherRangeIsInclusive()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(1), today.AddDays(5)); //Inclusive
            range.Overlaps(otherRange).Should().BeTrue();
        }

        [Fact]
        public void ShouldOverlapRangeWhenOtherRangeEndsAfterStartButBeforeEnd()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(-10), today.AddDays(5));
            range.Overlaps(otherRange).Should().BeTrue();
        }

        [Fact]
        public void ShouldOverlapRangeWhenOtherRangeStartsAfterStartAndEndsAfterEnd()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(5), today.AddDays(50));
            range.Overlaps(otherRange).Should().BeTrue();
        }

        [Fact]
        public void ShouldOverlapRangeWhenOtherRangeSTartsBeforeStartAndEndsAfterEnd()
        {
            var today = Clock.Local.Today;
            var range = new Range<DateTime>(today, today.AddDays(30));
            var otherRange = new Range<DateTime>(today.AddDays(-10), today.AddDays(50));
            range.Overlaps(otherRange).Should().BeTrue();
        }

        [Fact]
        public void ShouldNotInclude_SmallerThanStart()
        {
            var range = new Range<int>(1, 10); 
           range.Includes(range.Start-1).Should().BeFalse();
        }

        [Fact]
        public void ShouldNotInclude_largerThanEnd()
        {
            var range = new Range<int>(1, 10);
            range.Includes(range.End + 1).Should().BeFalse();
        }

        [Fact]
        public void CanIterateOverIntegers()
        {
            var iteratorValues = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var range = new Range<int>(1, 10);
            range.ForEach(inc => inc+1, each => iteratorValues.Contains(each).Should().BeTrue());
            range.End.Should().Be(iteratorValues.Count);
        }

        [Fact]
        public void CanIterateOverDates()
        {
            var start = DateTime.Today;
            var iteratorValues = new List<DateTime>
            {
                start,
                start.AddDays(1),
                start.AddDays(2),
                start.AddDays(3),
                start.AddDays(4),
                start.AddDays(5)
            };
            var range = new DateTimeRange(DateTime.Today, DateTime.Today.AddDays(5));
            range.ForEach(inc => inc.AddDays(1), each => iteratorValues.Contains(each).Should().BeTrue());
        }

        [Fact]
        public void CanIterateOverMinutes()
        {
            var start = DateTime.Today;
            var iteratorValues = new List<DateTime>
            {
                start,
                start.AddMinutes(1),
                start.AddMinutes(2),
                start.AddMinutes(3),
                start.AddMinutes(4)
            };
            var range = new DateTimeRange(DateTime.Today, DateTime.Today.AddMinutes(4));
            range.ForEach(inc => inc.AddMinutes(1), each => iteratorValues.Contains(each).Should().BeTrue());
        }

        [Fact]
        public void CanIterateOverDoubles()
        {
            var iteratorValues = new List<double>
            {
                100.01, 101.01, 102.01, 103.01, 104.01, 105.01, 106.01
            };
            var range = new Range<double>(100.01,106.01);
            range.ForEach(inc => inc+1, each => iteratorValues.Contains(each).Should().BeTrue());
        }

        [Fact]
        public void CanIterateOverDecimals()
        {
            var iteratorValues = new List<decimal>
            {
                100.0m, 101.01m, 102.02m, 103.03m, 104.04m, 105.05m, 106.06m
            };
            var range = new Range<decimal>(100.0m, 100.06m);
            range.ForEach(inc => inc+1.01m, each => iteratorValues.Contains(each).Should().BeTrue());
        }

        [Fact]
        public void CanConstructWithIncrementor()
        {
            int[] expected = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var withIncrementor = Range<int>.Create(1, 9);
            var actual = new List<int>();
            withIncrementor.ForEach(actual.Add); 
            expected.Should().BeEquivalentTo(actual.ToArray());
        }
    }
}
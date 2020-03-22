using System;

namespace Xerris.DotNet.Core.Utilities
{

    public class DateTimeRange : Range<DateTime>
    {
        private DateTimeRange() :base(default(DateTime), default(DateTime))
        { } 

        public DateTimeRange(DateTime start, DateTime end) : base(start, end, dtTime => dtTime.AddDays(1))
        {
        }
    }
}
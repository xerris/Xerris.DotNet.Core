using System;

namespace Xerris.DotNet.Core.Utilities;

public class DateTimeRange : Range<DateTime>
{
    private DateTimeRange() : base(default, default)
    {
    }

    public DateTimeRange(DateTime start, DateTime end) : base(start, end, dtTime => dtTime.AddDays(1))
    {
    }

    public TimeSpan Difference() => End - Start;
}
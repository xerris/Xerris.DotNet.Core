using System;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core.Time;

public class TimezoneOffset
{
    private TimezoneOffset(int offset)
        => Offset = offset;

    public int Offset { get; }
    public static TimezoneOffset Pacific => new(-8);
    public static TimezoneOffset Mountain => new(-7);
    public static TimezoneOffset Central => new(-6);
    public static TimezoneOffset Eastern => new(-5);

    public DateTime TodayFrom(in DateTime utc)
        => From(utc).Earliest();

    public DateTime From(in DateTime utc)
        => Clock.Local.Now.IsDaylightSavingTime() ? utc.AddHours(Offset + 1) : utc.AddHours(Offset);

    public void TodayFromUtc()
        => TodayFrom(DateTime.Today.ToUniversalTime());
}
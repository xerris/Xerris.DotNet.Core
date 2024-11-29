using System;

namespace Xerris.DotNet.Core.Time;

public static class Clock
{
    private static IClock local;

    private static IClock mountainStandardTime;

    private static IClock mountainTime;

    private static IClock utc;

    private static IClock australianEastern;

    public static IClock Local => local ??= new TimeZoneClock(TimeZoneInfo.Local);

    public static IClock MountainStandardTime => mountainStandardTime ??=
        new TimeZoneClock(TimeZones.MountainStandardTimeZone);

    public static IClock MountainTime =>
        mountainTime ??= new TimeZoneClock(TimeZones.MountainTimeZone);

    public static IClock CentralTime =>
        mountainTime ??= new TimeZoneClock(TimeZones.CentralTimeZone);

    public static IClock AustralianEasternTime =>
        australianEastern ??= new TimeZoneClock(TimeZones.AustralianEastern);

    public static IClock Utc => utc ??= new TimeZoneClock(TimeZoneInfo.Utc);

    public static DateTime EndOfTime => new(9999, 12, 31, 23, 59, 59, 999);

    public static bool AllowClockManipulation
    {
        get => ClockManager.AllowClockManipulation;
        set => ClockManager.AllowClockManipulation = value;
    }
}
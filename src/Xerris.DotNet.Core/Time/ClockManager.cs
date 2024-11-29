using System;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core.Time;

internal static class ClockManager
{
    static ClockManager()
    {
        AllowClockManipulation = true;
    }

    private static DateTime? FrozenTime { get; set; }
    internal static bool AllowClockManipulation { get; set; }

    public static DateTime Now
    {
        get
        {
            if (AllowClockManipulation) return FrozenTime ?? DateTime.Now.TruncateMilliseconds();
            return DateTime.Now.TruncateMilliseconds();
        }
    }

    internal static DateTime Today => Now.Date;

    public static void Initialize(bool allowClockManipulation)
    {
        AllowClockManipulation = allowClockManipulation;
    }

    internal static void Freeze()
    {
        Freeze(DateTime.Now.TruncateMilliseconds());
    }

    internal static void Freeze(DateTime dateTime)
    {
        FrozenTime = dateTime;
    }

    internal static void Thaw()
    {
        FrozenTime = null;
    }
}
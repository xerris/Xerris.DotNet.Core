using System;

namespace Xerris.DotNet.Core.Core
{
    public static class Clock
    {
        private static IClock local;

        private static IClock mountainStandardTime;

        private static IClock mountainTime;

        private static IClock utc;
        public static IClock Local => local ??= new TimeZoneClock(TimeZoneInfo.Local);

        public static IClock MountainStandardTime => mountainStandardTime ??= 
            new TimeZoneClock(TimeZones.MountainStandardTimeZone);

        public static IClock MountainTime =>
            mountainTime ??= new TimeZoneClock(TimeZones.MountainTimeZone);

        public static IClock Utc => utc ??= new TimeZoneClock(TimeZoneInfo.Utc);

        public static DateTime EndOfTime => new DateTime(9999, 12, 31, 23, 59, 59,999);

        public static bool AllowClockManipulation
        {
            get => ClockManager.AllowClockManipulation;
            set => ClockManager.AllowClockManipulation = value;
        }
    }
}
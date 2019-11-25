using System;

namespace Xerris.DotNet.Core.Core
{
    public static class TimeZones
    {
        /// <summary>
        ///     Does not use Daylight savings time
        /// </summary>
        public static readonly TimeZoneInfo MountainStandardTimeZone =
            TimeZoneInfo.CreateCustomTimeZone("Real Mountain Standard Time", new TimeSpan(-7, 0, 0),
                "Real Mountain Standard Time", "Real Mountain Standard Time");

        /// <summary>
        ///     Uses daylight savings time
        /// </summary>
        public static readonly TimeZoneInfo MountainTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
    }
}
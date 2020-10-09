using System;

namespace Xerris.DotNet.Core.Time
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
            FindSystemTimeZone("Mountain Standard Time", "America/Edmonton"); 
        
        public static readonly TimeZoneInfo CentralTimeZone =
            FindSystemTimeZone("Central Standard Time", "America/Regina");
        
        private static TimeZoneInfo FindSystemTimeZone(string windowsTimeZones, string ianaTimeZone)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZones);
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById(ianaTimeZone);
            }
        }
    }
}
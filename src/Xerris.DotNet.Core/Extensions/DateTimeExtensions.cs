using System;

namespace Xerris.DotNet.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimestammp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public static DateTime TruncateMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}
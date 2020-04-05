using System;
using System.Globalization;

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
            return new DateTime(dateTime.Year,dateTime.Month,dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
            //return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }

        public static DateTime ToDate(this string dateString, string format)
        {
            var provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(dateString, format, provider);
        }

        public static string Formatted(this DateTime date, string format)
        {
            return date.ToString("yyyyMMdd");
        }
    }
}
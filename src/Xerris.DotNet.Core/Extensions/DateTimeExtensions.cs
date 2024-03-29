﻿using System;
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

        public static string Formatted(this DateTime date, string format = "yyyyMMdd")
        {
            return date.ToString(format);
        }
        
        public static DateTime Latest(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59, 999);
        }
        
        public static DateTime Earliest(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }
    }
}
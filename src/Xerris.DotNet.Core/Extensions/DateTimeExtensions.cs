using System;
using System.Globalization;

namespace Xerris.DotNet.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToTimestamp(this DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    public static DateTime TruncateMilliseconds(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
            dateTime.Second, dateTime.Kind);
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

    public static DateTime ReduceMillisecondPrecision(this DateTime toTruncate)
    {
        var milliseconds = toTruncate.Millisecond;
        return new DateTime(toTruncate.Ticks - toTruncate.Ticks % TimeSpan.TicksPerSecond, toTruncate.Kind)
            .AddMilliseconds(milliseconds);
    }

    public static DateTime UtcTruncateSeconds(this DateTime timeToTruncate)
    {
        return new DateTime(timeToTruncate.Year, timeToTruncate.Month, timeToTruncate.Day, timeToTruncate.Hour,
            timeToTruncate.Minute, 0, 0, DateTimeKind.Utc);
    }

    public static DateTime UtcTruncateMilliseconds(this DateTime timeToTruncate)
    {
        return new DateTime(timeToTruncate.Year, timeToTruncate.Month, timeToTruncate.Day, timeToTruncate.Hour,
            timeToTruncate.Minute, timeToTruncate.Second, 0,
            DateTimeKind.Utc);
    }

    public static double ConvertToUnixTimeStamp(this DateTime dateTime)
    {
        var utcDate = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        var dateTimeOffset = new DateTimeOffset(utcDate);

        return dateTimeOffset.ToUnixTimeMilliseconds();
    }

    public static DateTime ConvertFromUnixTimeStamp(this double timeStamp)
    {
        return DateTime.UnixEpoch.AddMilliseconds(timeStamp);
    }
}
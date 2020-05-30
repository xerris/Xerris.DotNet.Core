using System;

namespace Xerris.DotNet.Core.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime Earliest(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }
    }
}
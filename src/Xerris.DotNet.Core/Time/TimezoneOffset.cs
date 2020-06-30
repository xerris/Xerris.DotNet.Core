using System;
using Xerris.DotNet.Core.Utilities;

namespace Xerris.DotNet.Core.Time
{
    public class TimezoneOffset
    {
        public int Offset { get; }

        private TimezoneOffset(int offset)
        {
            Offset = offset;
        }

        public static TimezoneOffset Pacific => new TimezoneOffset(-8);
        public static TimezoneOffset Mountain => new TimezoneOffset(-7);
        public static TimezoneOffset Central => new TimezoneOffset(-6);
        public static TimezoneOffset Eastern => new TimezoneOffset(-5);

        public DateTime TodayFrom(in DateTime utc)
        {
            return From(utc).Earliest();
        }

        public DateTime From(in DateTime utc)
        {
            return Clock.Local.Now.IsDaylightSavingTime() ? utc.AddHours(Offset + 1) : utc.AddHours(Offset);
        }

        public void TodayFromUtc() => TodayFrom(DateTime.Today.ToUniversalTime());
    }
}
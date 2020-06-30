using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xerris.DotNet.Core.Json
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string format;

        public DateTimeJsonConverter():this("yyyy-MM-ddTHH:mm:ss.fffZ")
        {
        }

        public DateTimeJsonConverter(string format)
        {
            this.format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.ParseExact(reader.GetString(), format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString(format));
        }
    }
}
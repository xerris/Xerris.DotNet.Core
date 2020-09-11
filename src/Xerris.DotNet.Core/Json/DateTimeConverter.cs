using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace Xerris.DotNet.Core.Json
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string format= "yyyy-MM-ddTHH:mm:ss.fffZ";

        public DateTimeJsonConverter(): this(format: "yyyy-MM-ddTHH:mm:ss.fffZ")
        {
        }

        public DateTimeJsonConverter(string format)
        {
            this.format = format;
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToUniversalTime().ToString(format));
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            Debug.Assert(objectType == typeof(DateTime));
            return DateTime.ParseExact(reader.Value.ToString(), format, CultureInfo.InvariantCulture);
        }
    }
}
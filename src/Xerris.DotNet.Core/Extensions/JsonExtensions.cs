using System.Text.Json;

namespace Xerris.DotNet.Core.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = true,
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        public static string ToJson<T>(this T subject, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(subject, options??DefaultSerializerOptions);
        }

        public static T FromJson<T>(this string json, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<T>(json, options??DefaultSerializerOptions);
        }
    }
}
using Newtonsoft.Json;

namespace Xerris.DotNet.Core.Core.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T FromJson<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Xerris.DotNet.Core.Extensions
{
    public static class JsonExtensions
    {
        private static readonly DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = ContractResolver,
            Formatting = Formatting.Indented
        };
        
        public static readonly JsonSerializerSettings DefaultCaseSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        
        public static string ToJson<T>(this T item, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(item, settings??Settings);
        }
        
        public static T FromJson<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
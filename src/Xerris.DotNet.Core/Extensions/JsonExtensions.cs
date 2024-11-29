using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Xerris.DotNet.Core.Extensions;

public static class JsonExtensions
{
    private static readonly DefaultContractResolver ContractResolver = new()
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };

    private static readonly JsonSerializerSettings Settings = new()
    {
        ContractResolver = ContractResolver
    };

    public static string ToJson<T>(this T item, JsonSerializerSettings settings = null)
        => JsonConvert.SerializeObject(item, settings ?? Settings);

    public static T FromJson<T>(this string data, JsonSerializerSettings settings = null)
        => JsonConvert.DeserializeObject<T>(data, settings ?? Settings);
}
using Newtonsoft.Json;

namespace Xerris.DotNet.Core.Serialization
{
    public class SerializationSettings
    {
        public static readonly JsonSerializerSettings LongNameSerializerSettings = new JsonSerializerSettings
            {ContractResolver = new LongNameContractResolver()};
    }
}
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.Cache;

namespace Xerris.DotNet.Core;

public sealed class ConfigureServiceCollection
{
    private readonly IServiceCollection collection;

    public ConfigureServiceCollection(IServiceCollection collection)
    {
        this.collection = collection;
    }

    public void Initialize()
    {
        collection.AddSingleton<ICache>(new WaitToFinishMemoryCache());
    }
}
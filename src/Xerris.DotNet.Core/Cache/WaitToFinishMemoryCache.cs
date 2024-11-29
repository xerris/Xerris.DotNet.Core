using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Xerris.DotNet.Core.Cache;

public class WaitToFinishMemoryCache : ICache
{
    private readonly int absoluteExpirationInMinutes;

    private readonly ConcurrentDictionary<object, SemaphoreSlim> locks = new();
    private readonly int slidingExpirationInMinutes;
    private MemoryCache cache = new(new MemoryCacheOptions { SizeLimit = 1000 });

    public WaitToFinishMemoryCache(int slidingExpirationInMinutes = 2, int absoluteExpirationInMinutes = 15)
    {
        this.slidingExpirationInMinutes = slidingExpirationInMinutes;
        this.absoluteExpirationInMinutes = absoluteExpirationInMinutes;
    }

    public async Task<TItem> GetOrCreate<TItem>(object key, Func<Task<TItem>> createItem)
    {
        if (cache.TryGetValue(key, out TItem cacheEntry)) return cacheEntry;
        var myLock = locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await myLock.WaitAsync();
        try
        {
            if (!cache.TryGetValue(key, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await createItem();
                cache.Set(key, cacheEntry, CreateCacheOptions());
            }
        }
        finally
        {
            myLock.Release();
        }

        return cacheEntry;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Purge()
    {
        if (cache.Count == 0) return;
        cache.Dispose();
        cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
    }

    private MemoryCacheEntryOptions CreateCacheOptions()
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSize(1) //Size amount
            //Priority on removing when reaching size limit (memory pressure)
            .SetPriority(CacheItemPriority.High)
            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpirationInMinutes))
            // Remove from cache after this time, regardless of sliding expiration
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpirationInMinutes));
        return cacheEntryOptions;
    }
}
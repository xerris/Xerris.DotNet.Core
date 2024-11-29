using System;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Cache
{
    /*
     * Disabled Cache
     */
    public class DisabledCache : ICache
    {
        public Task<TItem> GetOrCreate<TItem>(object key, Func<Task<TItem>> createItem)
            => createItem.Invoke();
    }
}
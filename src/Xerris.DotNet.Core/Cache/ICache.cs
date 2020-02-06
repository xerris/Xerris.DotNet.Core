using System;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Cache
{
    public interface ICache
    {
        Task<TItem> GetOrCreate<TItem>(object key, Func<Task<TItem>> createItem);
    }
}
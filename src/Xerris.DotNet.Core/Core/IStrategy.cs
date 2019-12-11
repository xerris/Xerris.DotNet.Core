using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Core
{
    public interface IStrategy<T>
    {
        Task<T> RunAsync(T subject);
    }
}
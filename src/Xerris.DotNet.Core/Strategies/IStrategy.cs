using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Strategies
{
    public interface IStrategy<T>
    {
        Task<T> RunAsync(T subject);
    }
}
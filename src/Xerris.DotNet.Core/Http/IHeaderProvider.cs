using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Http
{
    public interface IHeaderProvider
    {
        Task<(string name, string value)> GetHeaderAsync();
    }
}
using System.Threading.Tasks;
using Serilog;

namespace Xerris.DotNet.Core.Data
{
    public interface IUnitOfWorkProvider 
    {
        Task<IUnitOfWork> CreateAsync();
    }
    
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly IConnectionBuilder builder;

        public UnitOfWorkProvider(IConnectionBuilder builder)
        {
            this.builder = builder;
        }
        
        public async Task<IUnitOfWork> CreateAsync()
        {
            Log.Debug("Provider is about to request a new connection");
            return new UnitOfWork(await builder.CreateConnectionAsync().ConfigureAwait(false));
        }
    }
}
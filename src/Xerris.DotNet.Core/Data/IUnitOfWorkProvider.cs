using System.Threading.Tasks;
using Serilog;

namespace Xerris.DotNet.Core.Data
{
  public interface IUnitOfWorkProvider
    {
        Task<IUnitOfWork> CreateTransactional();
        Task<IUnitOfWork> CreateNonTransactional();
    }
    
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly IConnectionBuilder builder;

        public UnitOfWorkProvider(IConnectionBuilder builder)
            => this.builder = builder;
        
        public async Task<IUnitOfWork> CreateTransactional()
        {
            Log.Debug("Provider is about to request a new connection");
            var connection = await builder.CreateConnectionAsync().ConfigureAwait(false);
            connection.Open();
            return new UnitOfWork(connection);
        }

        public async Task<IUnitOfWork> CreateNonTransactional()
        {
            Log.Debug("Provider is about to request a new connection");
            var connection = await builder.CreateConnectionAsync().ConfigureAwait(false);
            connection.Open();
            return new ReadonlyUnitOfWork(connection);
        }
    }
}
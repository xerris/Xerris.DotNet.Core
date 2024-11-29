using System.Data;

namespace Xerris.DotNet.Core.Data
{
    public class ReadonlyUnitOfWork : IUnitOfWork
    {
        public IDbTransaction Transaction => null;
        public IDbConnection Connection { get; }

        public ReadonlyUnitOfWork(IDbConnection connection)
            => Connection = connection;

        public void Dispose()
            => Connection?.Dispose();

        public void Commit()
        {
        }

        public void Rollback()
        {
        }
    }
}
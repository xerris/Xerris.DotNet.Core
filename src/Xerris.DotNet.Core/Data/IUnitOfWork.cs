using System;
using System.Data;
using Serilog;

namespace Xerris.DotNet.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; }
        IDbConnection Connection { get; }
        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;
        private bool committed = false;
        private bool disposed;

        public UnitOfWork(IDbConnection connection)
        {
            this.connection = connection;
            transaction = connection.BeginTransaction();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                RollBackUncommited();
                transaction?.Dispose();
                connection?.Dispose();
            }
            disposed = true;
        }

        private void RollBackUncommited()
        {
            if (committed) return;
            try
            {
                Rollback();
            }
            catch (InvalidOperationException)
            {
                Log.Warning("Attempted rollback of transaction in dispose.");
            }
            catch (Exception e)
            {
                // if we can't roll back here we have issues
                Log.Warning(e, "Unable to rollback transaction in dispose.");
            }
        }

        public void Commit()
        {
            try
            {
                Log.Debug("Committing unit of work");
                transaction.Commit();
                committed = true;
                Log.Debug("Committing successful");
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to commit unit of work");
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public IDbTransaction Transaction => transaction;
        public IDbConnection Connection => connection;
    }
}
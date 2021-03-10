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
        private bool committed = false;
        private bool disposed;

        public IDbTransaction Transaction { get; }
        public IDbConnection Connection { get; }

        public UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
            Transaction = connection.BeginTransaction();
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
                if (!committed)
                {
                    try
                    {
                        Rollback();
                    }
                    catch (InvalidOperationException)
                    {
                        Log.Warning("Attempted rollback of transaction in dispose.");
                    }
                    catch(Exception e)
                    {
                        // if we can't roll back here we have issues
                        Log.Warning(e,"Unable to rollback transaction in dispose.");
                    }
                }
                Transaction?.Dispose();
                Connection?.Dispose();
            }
            disposed = true;
        }

        public void Commit()
        {
            try
            {
                Log.Debug("Committing unit of work");
                Transaction.Commit();
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
            Transaction.Rollback();
        }
    }
}
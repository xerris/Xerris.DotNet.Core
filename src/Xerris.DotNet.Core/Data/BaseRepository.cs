using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Xerris.DotNet.Core.Data
{
public abstract class BaseRepository
    {
        private readonly IConnectionBuilder connectionBuilder;

        protected BaseRepository(IConnectionBuilder connectionBuilder)
        {
            this.connectionBuilder = connectionBuilder;
        }

        protected async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = await connectionBuilder.CreateConnectionAsync();
            connection.Open();
            return connection;
        }
        
        protected async Task<int> ExecuteAsync(string sql, object parameters = null, IDbConnection connection = null,
            IDbTransaction transaction = null)
        {
            if (connection != null)
            {
                return await connection.ExecuteAsync(sql, parameters, transaction).ConfigureAwait(false);
            }

            using (connection = await CreateConnectionAsync())
            {
                return await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection != null)
            {
                return await connection.QueryAsync<T>(sql, parameters, transaction).ConfigureAwait(false);
            }

            using (connection = await CreateConnectionAsync())
            {
                return await connection.QueryAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }

        protected async Task<T> QuerySingleAsync<T>(string sql, object parameters = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection != null)
            {
                return await connection.QuerySingleAsync<T>(sql, parameters, transaction).ConfigureAwait(false);
            }

            using (connection = await CreateConnectionAsync())
            {
                return await connection.QuerySingleAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }

        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection != null)
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters, transaction).ConfigureAwait(false);
            }

            using (connection = await CreateConnectionAsync())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }

        protected async Task<T> InTransactionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> func)
        {
            using var connection = await CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await func(connection,transaction);
                transaction.Commit();
                return result;
            }
            catch 
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
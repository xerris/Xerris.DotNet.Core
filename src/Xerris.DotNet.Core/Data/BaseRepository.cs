using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using Serilog;

namespace Xerris.DotNet.Core.Data;

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

    protected async Task<IDbConnection> CreateReadonlyConnectionAsync()
    {
        var connection = await connectionBuilder.CreateReadConnectionAsync();
        if (connection.State != ConnectionState.Open) connection.Open();

        return connection;
    }

    protected async Task<int> ExecuteAsync(string sql, object parameters = null, IDbConnection connection = null,
        IDbTransaction transaction = null)
    {
        if (connection != null)
            return await connection.ExecuteAsync(sql, parameters, transaction).ConfigureAwait(false);

        using (var conn = await CreateConnectionAsync())
        {
            return await conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null,
        IDbConnection connection = null, IDbTransaction transaction = null, int retries = 1,
        [CallerMemberName] string callingMethod = null)
    {
        return await QueryWithRetry(async () =>
        {
            if (connection != null)
                return await connection.QueryAsync<T>(sql, parameters, transaction).ConfigureAwait(false);

            using (var conn = await CreateReadonlyConnectionAsync())
            {
                return await conn.QueryAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }, retries, callingMethod);
    }

    protected async Task<T> QuerySingleAsync<T>(string sql, object parameters = null,
        IDbConnection connection = null, IDbTransaction transaction = null, int retries = 1,
        [CallerMemberName] string callingMethod = null)
    {
        return await QueryWithRetry(async () =>
        {
            if (connection != null)
                return await connection.QuerySingleAsync<T>(sql, parameters, transaction).ConfigureAwait(false);

            using (var conn = await CreateReadonlyConnectionAsync())
            {
                return await conn.QuerySingleAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }, retries, callingMethod);
    }

    protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null,
        IDbConnection connection = null, IDbTransaction transaction = null, int retries = 1,
        [CallerMemberName] string callingMethod = null)
    {
        return await QueryWithRetry(async () =>
        {
            if (connection != null)
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters, transaction)
                    .ConfigureAwait(false);

            using (var conn = await CreateReadonlyConnectionAsync())
            {
                return await conn.QuerySingleOrDefaultAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }, retries, callingMethod);
    }

    protected async Task<T> InTransactionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> func)
    {
        using var connection = await CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        try
        {
            var result = await func(connection, transaction);
            transaction.Commit();
            return result;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static async Task<T> QueryWithRetry<T>(Func<Task<T>> query, int retries, string callingMethod)
    {
        var attempt = 0;
        while (attempt < retries)
            try
            {
                return await query().ConfigureAwait(false);
            }
            catch (Exception)
            {
                attempt++;
                if (attempt == retries) throw;
                await Task.Delay(100);
                Log.Information($"Query attempt {attempt} of {retries} for {callingMethod}");
            }

        throw new ApplicationException("The system has entered an invalid state querying the database.");
    }
}
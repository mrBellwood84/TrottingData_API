using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Persistence.Services;

/// <summary>
///     Serves as the base class for database services, providing core data access methods
///     using Dapper and MySQL. Supports both simple flat entities and virtual complex queries
///     that can be overridden for custom object hydration.
/// </summary>
/// <typeparam name="TEntity">The flat database entity representation.</typeparam>
/// <typeparam name="TComplex">The rich, complex model representation representing combined data.</typeparam>
/// <param name="config">The application configuration containing connection string definitions.</param>
public class BaseDbService<TEntity, TComplex>(IConfiguration config)
{
    /// <summary>
    ///     Creates, opens, and returns a new MySQL database connection.
    ///     Restricted to inheriting services within the same assembly.
    /// </summary>
    /// <returns>An active, opened <see cref="MySqlConnection" /> instance.</returns>
    private protected async Task<MySqlConnection> CreateConnection()
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }

    /// <summary>
    ///     Queries the database for a single, flat entity based on the provided query and parameters.
    /// </summary>
    /// <param name="query">The SQL query string to execute.</param>
    /// <param name="param">The parameters to inject safely into the SQL query.</param>
    /// <returns>The retrieved <typeparamref name="TEntity" /> if found; otherwise, <see langword="null" />.</returns>
    protected async Task<TEntity?> QueryEntityAsync(string query, object param)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<TEntity>(query, param);
        return data;
    }

    /// <summary>
    ///     Queries the database for a single complex model.
    ///     Marked as <see langword="virtual" /> to allow specialized overriding for custom multi-mapping.
    /// </summary>
    /// <param name="query">The SQL query string to execute.</param>
    /// <param name="param">The parameters to inject safely into the SQL query.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model if found; otherwise, <see langword="null" />.</returns>
    protected virtual async Task<TComplex?> QueryComplexAsync(string query, object param)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<TComplex>(query, param);
        return data;
    }

    /// <summary>
    ///     Queries the database for a list of complex models.
    ///     Marked as <see langword="virtual" /> to allow specialized overriding for custom multi-mapping.
    /// </summary>
    /// <param name="query">The SQL query string to execute.</param>
    /// <returns>A list containing the populated <typeparamref name="TComplex" /> models.</returns>
    protected virtual async Task<List<TComplex>> QueryComplexListAsync(string query)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<TComplex>(query);
        return data.ToList();
    }
}
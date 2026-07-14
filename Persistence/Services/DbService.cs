using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Shared;
using Persistence.Exceptions;
using Persistence.Interfaces;

namespace Persistence.Services;

/// <summary>
///     A generic database service that handles standard CRUD operations for both flat entities and complex models.
///     Inherits from <see cref="DbConnection" /> to manage lifecycle of connections.
/// </summary>
/// <typeparam name="TEntity">The flat entity model mapping 1:1 with the database table.</typeparam>
/// <typeparam name="TComplex">The aggregated/nested model used for API responses.</typeparam>
/// <param name="configuration">The application configuration for retrieving database connection strings.</param>
/// <param name="policy">
///     The policy governing permissions (e.g., whether GetAll is allowed) for
///     <typeparamref name="TEntity" />.
/// </param>
public class DbService<TEntity, TComplex>(IConfiguration configuration, ModelPolicy<TEntity> policy)
    : DbConnection(configuration), IDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Gets the SQL query used to retrieve all IDs for this entity type.
    /// </summary>
    protected string QueryIds { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the SQL query used to retrieve all flat entities.
    /// </summary>
    protected string QueryEntity { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the SQL query used to retrieve a single flat entity by its ID.
    /// </summary>
    protected string QueryEntityById { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the SQL query used to retrieve all complex entities.
    /// </summary>
    protected string QueryComplex { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the SQL query used to retrieve a single complex entity by its ID.
    /// </summary>
    protected string QueryComplexById { get; init; } = string.Empty;

    /// <summary>
    ///     Retrieves all IDs for the entity from the database. This query bypasses any cache.
    /// </summary>
    /// <returns>A list of <see cref="IdModel" /> wrapping the primary keys.</returns>
    /// <exception cref="PersistenceMissingQueryException">Thrown when <see cref="QueryIds" /> has not been configured.</exception>
    public async Task<List<IdModel>> GetIdsAsync()
    {
        if (string.IsNullOrEmpty(QueryIds))
            throw new PersistenceMissingQueryException($"Missing QueryIds for {typeof(TComplex).Name}");

        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<IdModel>(QueryIds);
        return data.ToList();
    }

    /// <summary>
    ///     Retrieves all flat entities from the database, provided that the model's policy allows it.
    /// </summary>
    /// <returns>A list of <typeparamref name="TEntity" /> entities.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">Thrown when 'GetAll' is disallowed by policy, or the SQL query is empty.</exception>
    public async Task<List<TEntity>> GetAllEntityAsync()
    {
        if (!policy.AllowGetAll)
            throw new PersistenceQueryNotAllowedException($"GetAllEntity for {typeof(TEntity).Name} is disallowed");
        if (string.IsNullOrEmpty(QueryEntity))
            throw new PersistenceMissingQueryException($"Missing QueryEntity for {typeof(TEntity).Name}");

        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<TEntity>(QueryEntity);
        return data.ToList();
    }

    /// <summary>
    ///     Retrieves a single flat entity by its unique identifier.
    /// </summary>
    /// <param name="id">The GUID string of the entity.</param>
    /// <returns>The matching <typeparamref name="TEntity" /> entity, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">Thrown when <see cref="QueryEntityById" /> has not been configured.</exception>
    public async Task<TEntity?> GetEntityByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(QueryEntityById))
            throw new PersistenceMissingQueryException($"Missing QueryEntityById for {typeof(TEntity).Name}");

        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<TEntity>(QueryEntityById, new { Id = id });
        return data;
    }

    /// <summary>
    ///     Retrieves all complex representation models. Relies on internal mapping logic.
    /// </summary>
    /// <returns>A list of populated <typeparamref name="TComplex" /> models.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">Thrown when <see cref="QueryComplex" /> has not been configured.</exception>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        if (!policy.AllowGetAll)
            throw new PersistenceQueryNotAllowedException($"GetAllComplex for {typeof(TComplex).Name} is disallowed");
        if (string.IsNullOrEmpty(QueryComplex))
            throw new PersistenceMissingQueryException($"Missing QueryComplex for {typeof(TComplex).Name}");

        var data = await GetAllComplexLogicAsync();
        return data;
    }

    /// <summary>
    ///     Retrieves a specific complex model by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">Thrown when <see cref="QueryComplexById" /> has not been configured.</exception>
    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(QueryComplexById))
            throw new PersistenceMissingQueryException($"Missing QueryComplexById for {typeof(TComplex).Name}");

        var data = await GetComplexByIdLogicAsync(id);
        return data;
    }

    /// <summary>
    ///     Abstract-like virtual method that must be overridden in specific subclasses to resolve
    ///     complex mappings (such as multi-mapping 1:M or M:M relationships in Dapper).
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of complex models.</returns>
    /// <exception cref="PersistenceNotImplementedException">Thrown if called without a concrete override in a subclass.</exception>
    private protected virtual async Task<List<TComplex>> GetAllComplexLogicAsync()
    {
        throw new PersistenceNotImplementedException(
            $"GetAllComplex logic for {typeof(TComplex).Name} is not implemented");
    }

    /// <summary>
    ///     Abstract-like virtual method that must be overridden in specific subclasses to resolve
    ///     a single complex mapping by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task representing the asynchronous operation, containing the complex model or null.</returns>
    /// <exception cref="PersistenceNotImplementedException">Thrown if called without a concrete override in a subclass.</exception>
    private protected virtual async Task<TComplex?> GetComplexByIdLogicAsync(string id)
    {
        throw new PersistenceNotImplementedException(
            $"GetComplexByIdAsync logic for {typeof(TComplex).Name} is not implemented");
    }
}
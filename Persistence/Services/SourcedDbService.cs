using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Interfaces;
using Persistence.Exceptions;
using Persistence.Interfaces;

namespace Persistence.Services;

/// <summary>
///     An extended database service handling specialized lookup operations using an external source identifier.
///     Inherits from <see cref="DbService{TEntity, TComplex}" />.
/// </summary>
/// <typeparam name="TEntity">The flat entity model, which must implement <see cref="ISourcedEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated/nested model used for API responses.</typeparam>
/// <param name="configuration">The application configuration for retrieving database connection strings.</param>
public class SourcedDbService<TEntity, TComplex>(IConfiguration configuration)
    : DbService<TEntity, TComplex>(configuration), ISourcedDbService<TEntity, TComplex>
    where TEntity : ISourcedEntity where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Gets the SQL query used to retrieve a single flat entity by its external Source ID.
    /// </summary>
    protected string QueryEntityBySourceId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the SQL query used to retrieve a single complex entity by its external Source ID.
    /// </summary>
    protected string QueryComplexBySourceId { get; init; } = string.Empty;

    /// <summary>
    ///     Retrieves a single flat entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The matching <typeparamref name="TEntity" />, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="QueryEntityBySourceId" /> has not been configured.
    /// </exception>
    public async Task<TEntity?> GetEntityBySourceIdAsync(string sourceId)
    {
        if (string.IsNullOrEmpty(QueryEntityBySourceId))
            throw new PersistenceMissingQueryException($"Missing QueryEntityBySourceId for {typeof(TEntity).Name}");

        await using var connection = await CreateConnection();

        var data = await connection.QueryFirstOrDefaultAsync<TEntity>(QueryEntityBySourceId,
            new { SourceId = sourceId });
        return data;
    }

    /// <summary>
    ///     Retrieves a specific complex model by its external source identifier. Relies on internal mapping logic.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="QueryComplexBySourceId" /> has not been configured.
    /// </exception>
    public async Task<TComplex?> GetComplexBySourceIdAsync(string sourceId)
    {
        if (string.IsNullOrEmpty(QueryComplexBySourceId))
            throw new PersistenceMissingQueryException($"Missing QueryComplexBySourceId for {typeof(TComplex).Name}");

        var data = await GetComplexBySourceIdLogicAsync(sourceId);
        return data;
    }

    /// <summary>
    ///     Abstract-like virtual method that must be overridden in specific subclasses to resolve
    ///     complex multi-mappings (such as 1:M or M:M Dapper relationships) by Source ID.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>A task representing the asynchronous operation, containing the complex model or null.</returns>
    /// <exception cref="PersistenceNotImplementedException">Thrown if called without a concrete override in a subclass.</exception>
    private protected virtual Task<TComplex?> GetComplexBySourceIdLogicAsync(string sourceId)
    {
        throw new PersistenceNotImplementedException(
            $"GetComplexBySourceIdLogicAsync for {typeof(TComplex).Name} is not implemented");
    }
}
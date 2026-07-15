using Microsoft.Extensions.Configuration;
using Persistence.Exceptions;

namespace Persistence.Services;

/// <summary>
///     Represents a database service specialized in retrieving data based on an external source identifier.
///     Inherits single-retrieval capabilities and extends them with source-specific querying.
/// </summary>
/// <typeparam name="TEntity">The flat database entity representation.</typeparam>
/// <typeparam name="TComplex">The rich, complex model representation representing combined data.</typeparam>
/// <param name="config">The application configuration containing connection string definitions.</param>
public class ReadSourcedDbService<TEntity, TComplex>(IConfiguration config)
    : ReadSingleDbService<TEntity, TComplex>(config), IReadSourcedDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Gets the SQL statement used to retrieve a single simple entity by its external source ID.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectEntityBySourceId { get; } = string.Empty;

    /// <summary>
    ///     Gets the SQL statement used to retrieve a single complex model by its external source ID.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectComplexBySourceId { get; } = string.Empty;

    /// <summary>
    ///     Retrieves a single simple entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the entity.</param>
    /// <returns>The retrieved <typeparamref name="TEntity" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectEntityBySourceId" /> is not defined
    ///     by the subclass.
    /// </exception>
    public Task<TEntity?> GetEntityBySourceIdAsync(string sourceId)
    {
        if (string.IsNullOrEmpty(SqlSelectEntityBySourceId))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectEntityBySourceId)}' for {typeof(TEntity).Name} is missing!");

        var param = new { SourceId = sourceId };
        var data = QueryEntityAsync(SqlSelectEntityBySourceId, param);
        return data;
    }

    /// <summary>
    ///     Retrieves a single complex model by its external source identifier.
    ///     Uses the virtual helper to allow specialized multi-mapping configurations in subclasses.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the complex model.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectComplexBySourceId" /> is not defined
    ///     by the subclass.
    /// </exception>
    public Task<TComplex?> GetComplexBySourceIdAsync(string sourceId)
    {
        if (string.IsNullOrEmpty(SqlSelectComplexBySourceId))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectComplexBySourceId)}' for {typeof(TComplex).Name} is missing!");

        var param = new { SourceId = sourceId };
        var data = QueryComplexAsync(SqlSelectComplexBySourceId, param);
        return data;
    }
}
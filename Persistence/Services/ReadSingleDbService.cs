using Microsoft.Extensions.Configuration;
using Persistence.Exceptions;
using Persistence.Interfaces;

namespace Persistence.Services;

/// <summary>
///     Represents a database service specialized in retrieving a single instance of a model
///     (either as a flat database entity or a rich complex representation) by its unique identifier.
/// </summary>
/// <typeparam name="TEntity">The flat database entity representation.</typeparam>
/// <typeparam name="TComplex">The rich, complex model representation representing combined data.</typeparam>
/// <param name="config">The application configuration containing connection string definitions.</param>
public class ReadSingleDbService<TEntity, TComplex>(IConfiguration config)
    : BaseDbService<TEntity, TComplex>(config), IReadSingleDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Gets the SQL statement used to retrieve a single simple entity by its ID.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectEntityById { get; } = string.Empty;

    /// <summary>
    ///     Gets the SQL statement used to retrieve a single complex model by its ID.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectComplexById { get; } = string.Empty;


    // todo : rename to GetEntityById
    /// <summary>
    ///     Retrieves a single simple entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The retrieved <typeparamref name="TEntity" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectEntityById" /> is not defined by the
    ///     subclass.
    /// </exception>
    public Task<TEntity?> GetSingleEntityByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(SqlSelectEntityById))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectEntityById)}' for {typeof(TEntity).Name} is missing!");

        var param = new { Id = id };
        return QueryEntityAsync(SqlSelectEntityById, param);
    }

    // todo : rename to GetComplexById
    /// <summary>
    ///     Retrieves a single complex model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the complex model to retrieve.</param>
    /// <returns>The retrieved <typeparamref name="TComplex" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectComplexById" /> is not defined by
    ///     the subclass.
    /// </exception>
    public Task<TComplex?> GetSingleComplexByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(SqlSelectComplexById))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectComplexById)}' for {typeof(TComplex).Name} is missing!");

        var param = new { Id = id };
        return QueryComplexAsync(SqlSelectComplexById, param);
    }
}
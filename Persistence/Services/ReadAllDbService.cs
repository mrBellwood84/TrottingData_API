using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Shared;
using Persistence.Exceptions;
using Persistence.Interfaces;

namespace Persistence.Services;

/// <summary>
///     Represents a database service specialized in bulk retrieval operations,
///     providing methods to fetch collections of IDs, flat entities, or complex models.
/// </summary>
/// <typeparam name="TEntity">The flat database entity representation.</typeparam>
/// <typeparam name="TComplex">The rich, complex model representation representing combined data.</typeparam>
/// <param name="config">The application configuration containing connection string definitions.</param>
public class ReadAllDbService<TEntity, TComplex>(IConfiguration config)
    : ReadSingleDbService<TEntity, TComplex>(config), IReadAllDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Gets the SQL statement used to retrieve a list of all entity IDs wrapped in an <see cref="IdModel" />.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectIds { get; } = string.Empty;

    /// <summary>
    ///     Gets the SQL statement used to retrieve a list of all simple entities.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectEntities { get; } = string.Empty;

    /// <summary>
    ///     Gets the SQL statement used to retrieve a list of all complex models.
    ///     Must be overridden in inheriting service classes.
    /// </summary>
    protected virtual string SqlSelectComplex { get; } = string.Empty;

    /// <summary>
    ///     Retrieves all available unique identifiers for the entity type.
    /// </summary>
    /// <returns>A list of <see cref="IdModel" /> instances containing the active IDs.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectIds" /> is not defined by the
    ///     subclass.
    /// </exception>
    public async Task<List<IdModel>> GetIdsAsync()
    {
        if (string.IsNullOrEmpty(SqlSelectIds))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectIds)}' for {typeof(TEntity).Name} is missing!");

        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<IdModel>(SqlSelectIds);
        return data.ToList();
    }

    /// <summary>
    ///     Retrieves all instances of the simple entity from the database.
    /// </summary>
    /// <returns>A list containing all retrieved <typeparamref name="TEntity" /> instances.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectEntities" /> is not defined by the
    ///     subclass.
    /// </exception>
    public async Task<List<TEntity>> GetEntitiesAsync()
    {
        if (string.IsNullOrEmpty(SqlSelectEntities))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectEntities)}' for {typeof(TEntity).Name} is missing!");

        return await QueryEntityListAsync(SqlSelectEntities, new { });
    }

    /// <summary>
    ///     Retrieves all instances of the complex model from the database.
    ///     Uses the virtual list helper to allow for custom multi-mapping overrides.
    /// </summary>
    /// <returns>A list containing all populated <typeparamref name="TComplex" /> models.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="SqlSelectComplex" /> is not defined by the
    ///     subclass.
    /// </exception>
    public Task<List<TComplex>> GetComplexAsync()
    {
        if (string.IsNullOrEmpty(SqlSelectComplex))
            throw new PersistenceMissingQueryException(
                $"SQL statement '{nameof(SqlSelectComplex)}' for {typeof(TComplex).Name} is missing!");

        return QueryComplexListAsync(SqlSelectComplex);
    }
}
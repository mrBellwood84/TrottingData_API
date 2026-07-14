using Models.Interfaces;
using Models.Shared;
using Persistence.Exceptions;
using Persistence.Services;

namespace Persistence.Interfaces;

public interface ISourcedDbService<TEntity, TComplex> where TEntity : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a single flat entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The matching <typeparamref name="TEntity" />, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="SourcedDbService{TEntity,TComplex}.QueryEntityBySourceId" /> has not been configured.
    /// </exception>
    Task<TEntity?> GetEntityBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves a specific complex model by its external source identifier. Relies on internal mapping logic.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="SourcedDbService{TEntity,TComplex}.QueryComplexBySourceId" /> has not been configured.
    /// </exception>
    Task<TComplex?> GetComplexBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves all IDs for the entity from the database. This query bypasses any cache.
    /// </summary>
    /// <returns>A list of <see cref="IdModel" /> wrapping the primary keys.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="DbService{TEntity,TComplex}.QueryIds" /> has
    ///     not been configured.
    /// </exception>
    Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    ///     Retrieves all flat entities from the database, provided that the model's policy allows it.
    /// </summary>
    /// <returns>A list of <typeparamref name="TEntity" /> entities.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when 'GetAll' is disallowed by policy, or the SQL query is
    ///     empty.
    /// </exception>
    Task<List<TEntity>> GetAllEntityAsync();

    /// <summary>
    ///     Retrieves a single flat entity by its unique identifier.
    /// </summary>
    /// <param name="id">The GUID string of the entity.</param>
    /// <returns>The matching <typeparamref name="TEntity" /> entity, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="DbService{TEntity,TComplex}.QueryEntityById" /> has not been configured.
    /// </exception>
    Task<TEntity?> GetEntityByIdAsync(string id);

    /// <summary>
    ///     Retrieves all complex representation models. Relies on internal mapping logic.
    /// </summary>
    /// <returns>A list of populated <typeparamref name="TComplex" /> models.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="DbService{TEntity,TComplex}.QueryComplex" />
    ///     has not been configured.
    /// </exception>
    Task<List<TComplex>> GetAllComplexAsync();

    /// <summary>
    ///     Retrieves a specific complex model by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="DbService{TEntity,TComplex}.QueryComplexById" /> has not been configured.
    /// </exception>
    Task<TComplex?> GetComplexByIdAsync(string id);
}
using Models.Shared;
using Persistence.Exceptions;
using Persistence.Services;

namespace Persistence.Interfaces;

public interface IDbService<TSimple, TComplex>
{
    /// <summary>
    ///     Retrieves all IDs for the entity from the database. This query bypasses any cache.
    /// </summary>
    /// <returns>A list of <see cref="IdModel" /> wrapping the primary keys.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="DbService{TSimple,TComplex}.QueryIds" /> has
    ///     not been configured.
    /// </exception>
    public Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    ///     Retrieves all simple entities from the database, provided that the model's policy allows it.
    /// </summary>
    /// <returns>A list of <typeparamref name="TSimple" /> entities.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when 'GetAll' is disallowed by policy, or the SQL query is
    ///     empty.
    /// </exception>
    public Task<List<TSimple>> GetAllEntityAsync();

    /// <summary>
    ///     Retrieves a single simple entity by its unique identifier.
    /// </summary>
    /// <param name="id">The GUID string of the entity.</param>
    /// <returns>The matching <typeparamref name="TSimple" /> entity, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="DbService{TSimple,TComplex}.QueryEntityById" /> has not been configured.
    /// </exception>
    public Task<TSimple?> GetEntityByIdAsync(string id);

    /// <summary>
    ///     Retrieves all complex representation models. Relies on internal mapping logic.
    /// </summary>
    /// <returns>A list of populated <typeparamref name="TComplex" /> models.</returns>
    /// <exception cref="PersistenceQueryNotAllowedException">Thrown when the query is disallowed by policy.</exception>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when <see cref="DbService{TSimple,TComplex}.QueryComplex" />
    ///     has not been configured.
    /// </exception>
    public Task<List<TComplex>> GetAllComplexAsync();

    /// <summary>
    ///     Retrieves a specific complex model by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="DbService{TSimple,TComplex}.QueryComplexById" /> has not been configured.
    /// </exception>
    public Task<TComplex?> GetComplexByIdAsync(string id);
}
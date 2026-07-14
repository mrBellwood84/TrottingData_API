using Models.Interfaces;
using Models.Shared;

namespace Application.Repository.Interfaces;

public interface ISourcedRepositoryService<TEntity, TComplex> where TEntity : ISourcedEntity where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a single flat entity by its external Source ID, populating both internal and source caches on a miss.
    /// </summary>
    /// <param name="sourceId">The external system identifier for the entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the entity if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    Task<TEntity?> GetEntityBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves a single complex model by its external Source ID, populating both internal and source caches on a miss.
    /// </summary>
    /// <param name="sourceId">The external system identifier for the complex entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the complex model if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    Task<TComplex?> GetComplexBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves all available record IDs directly from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="IdModel"/>s.</returns>
    Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    ///     Retrieves all flat entities, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of flat entities.</returns>
    Task<List<TEntity>> GetAllSimplesAsync();

    /// <summary>
    ///     Retrieves a single flat entity by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the entity if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    Task<TEntity?> GetSimpleByIdAsync(string id);

    /// <summary>
    ///     Retrieves all complex models, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of complex models.</returns>
    Task<List<TComplex>> GetAllComplexAsync();

    /// <summary>
    ///     Retrieves a single complex model by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the complex model if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    Task<TComplex?> GetComplexByIdAsync(string id);
}
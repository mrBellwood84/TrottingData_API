using Models.Interfaces;
using Models.Shared;

namespace Application.Repository.Interfaces;

public interface IRepositoryService<TEntity, TComplex> where TEntity : IEntity where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves all available record IDs directly from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="IdModel" />s.</returns>
    public Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    ///     Retrieves all flat entities, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of flat entities.</returns>
    public Task<List<TEntity>> GetAllEntityAsync();

    /// <summary>
    ///     Retrieves a single flat entity by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the entity if found;
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public Task<TEntity?> GetEntityByIdAsync(string id);

    /// <summary>
    ///     Retrieves all complex models, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of complex models.</returns>
    public Task<List<TComplex>> GetAllComplexAsync();

    /// <summary>
    ///     Retrieves a single complex model by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the complex model if found;
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public Task<TComplex?> GetComplexByIdAsync(string id);
}
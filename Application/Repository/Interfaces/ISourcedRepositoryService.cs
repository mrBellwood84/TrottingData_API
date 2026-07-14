using Models.Interfaces;
using Models.Shared;

namespace Application.Repository.Interfaces;

public interface ISourcedRepositoryService<TEntity, TComplex> : IRepositoryService<TEntity, TComplex>
    where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
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
}
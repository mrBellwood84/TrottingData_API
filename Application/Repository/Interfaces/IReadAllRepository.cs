using Models.Interfaces;

namespace Application.Repository.Interfaces;

public interface IReadAllRepository<TEntity, TComplex>
    : IReadSingleRepository<TEntity, TComplex>
    where TEntity : IEntity
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a list of all identity models, subject to policy restrictions.
    /// </summary>
    Task<List<string>> GetAllIdsAsync();

    /// <summary>
    ///     Retrieves all flat entities, returning the fully loaded cache if available,
    ///     otherwise fetching from the database and populating the cache.
    /// </summary>
    Task<List<TEntity>> GetAllEntitiesAsync();

    /// <summary>
    ///     Retrieves all complex domain models, returning the fully loaded cache if available,
    ///     otherwise fetching from the database and populating the cache.
    /// </summary>
    Task<List<TComplex>> GetAllComplexAsync();
}
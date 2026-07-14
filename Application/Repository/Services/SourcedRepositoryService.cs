using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Specialized repository service that manages data flow for external source-mapped entities,
///     synchronizing data across both standard and source-keyed in-memory caches.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type. Must implement <see cref="ISourcedEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type. Must implement <see cref="ISourcedEntity" />.</typeparam>
/// <param name="entityCache">The cache service responsible for storing flat entities by their primary ID.</param>
/// <param name="complexCache">The cache service responsible for storing complex models by their primary ID.</param>
/// <param name="sourceEntityCache">The cache service responsible for storing flat entities by their external Source ID.</param>
/// <param name="sourceComplexCache">The cache service responsible for storing complex models by their external Source ID.</param>
/// <param name="dbService">The database persistence service handling direct SQL operations for source-mapped entities.</param>
public class SourcedRepositoryService<TEntity, TComplex>(
    CacheService<TEntity> entityCache, 
    CacheService<TComplex> complexCache,
    SourcedCacheService<TEntity> sourceEntityCache,
    SourcedCacheService<TComplex> sourceComplexCache,
    ISourcedDbService<TEntity, TComplex> dbService) 
    : RepositoryService<TEntity, TComplex>(entityCache, complexCache, dbService), ISourcedRepositoryService<TEntity, TComplex> 
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
    public async Task<TEntity?> GetEntityBySourceIdAsync(string sourceId)
    {
        var cacheData = await sourceEntityCache.Get(sourceId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetEntityBySourceIdAsync(sourceId);
        if (dbData is null) return default;
        
        // Multi-indexing: Populate both caches to maximize future cache hits
        await sourceEntityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a single complex model by its external Source ID, populating both internal and source caches on a miss.
    /// </summary>
    /// <param name="sourceId">The external system identifier for the complex entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the complex model if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public async Task<TComplex?> GetComplexBySourceIdAsync(string sourceId)
    {
        var cacheData = await sourceComplexCache.Get(sourceId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetComplexBySourceIdAsync(sourceId);
        if (dbData is null) return default;
        
        await sourceComplexCache.Set(dbData);
        return dbData;
    }
}
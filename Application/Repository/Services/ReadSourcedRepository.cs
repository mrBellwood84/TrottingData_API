using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Persistence.Services;

namespace Application.Repository.Services;

/// <summary>
///     Provides a repository implementation for entities and complex models that originate
///     from an external source, enabling lookups via their external source identifier
///     with dedicated cache-aside management.
/// </summary>
public class ReadSourcedRepository<TEntity, TComplex>(
    CacheService<TEntity> entityCache,
    CacheService<TComplex> complexCache,
    SourcedCacheService<TEntity> sourcedEntityCache,
    SourcedCacheService<TComplex> sourcedComplexCache,
    IReadSourcedDbService<TEntity, TComplex> dbService)
    : ReadSingleRepository<TEntity, TComplex>(entityCache, complexCache, dbService),
        IReadSourcedRepository<TEntity, TComplex> where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its external source identifier, checking the sourced cache first.
    /// </summary>
    public async Task<TEntity?> GetEntityBySourceIdAsync(string sourceId)
    {
        var cacheData = await sourcedEntityCache.Get(sourceId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityBySourceIdAsync(sourceId);
        if (dbData is not null) await sourcedEntityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex model by its external source identifier, checking the sourced cache first.
    /// </summary>
    public async Task<TComplex?> GetComplexBySourceIdAsync(string sourceId)
    {
        var cacheData = await sourcedComplexCache.Get(sourceId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexBySourceIdAsync(sourceId);
        if (dbData is not null) await sourcedComplexCache.Set(dbData);
        return dbData;
    }
}
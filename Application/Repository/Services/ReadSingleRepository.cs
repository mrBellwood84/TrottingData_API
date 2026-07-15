using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Provides a generic repository base that implements the cache-aside pattern
///     for retrieving single flat entities and complex domain models by their identifier.
/// </summary>
public class ReadSingleRepository<TEntity, TComplex>(
    CacheService<TEntity> entityCache,
    CacheService<TComplex> complexCache,
    IReadSingleDbService<TEntity, TComplex> dbService)
    : IReadSingleRepository<TEntity, TComplex> where TEntity : IEntity
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its identifier, checking the cache first before querying the database.
    /// </summary>
    public async Task<TEntity?> GetEntityByIdAsync(string id)
    {
        var cacheData = await entityCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetSingleEntityByIdAsync(id);
        if (dbData is not null) await entityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex model by its identifier, checking the cache first before querying the database.
    /// </summary>
    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetSingleComplexByIdAsync(id);
        if (dbData is not null) await complexCache.Set(dbData);
        return dbData;
    }
}
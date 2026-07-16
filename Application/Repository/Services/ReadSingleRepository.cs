using Application.Cache.Interfaces;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Provides a generic repository base that implements the cache-aside pattern
///     for retrieving single flat entities and complex domain models by their identifier.
/// </summary>
public class ReadSingleRepository<TEntity, TComplex>(
    IReadSingleCache<TEntity> entityCache,
    IReadSingleCache<TComplex> complexCache,
    IReadSingleDbService<TEntity, TComplex> dbService)
    : IReadSingleRepository<TEntity, TComplex>
    where TEntity : IEntity
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its identifier, checking the cache first before querying the database.
    /// </summary>
    public async Task<TEntity?> GetEntityByIdAsync(string id)
    {
        var cacheData = await entityCache.GetAsync(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByIdAsync(id);
        if (dbData is not null) await entityCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex model by its identifier, checking the cache first before querying the database.
    /// </summary>
    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.GetAsync(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByIdAsync(id);
        if (dbData is not null) await complexCache.SetAsync(dbData);
        return dbData;
    }
}
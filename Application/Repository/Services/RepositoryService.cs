using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Manages data flow between in-memory caches and the database using a Cache-Aside pattern.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type. Must implement <see cref="IEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type. Must implement <see cref="IEntity" />.</typeparam>
/// <param name="entityCache">The cache service responsible for storing flat entity representations.</param>
/// <param name="complexCache">The cache service responsible for storing aggregated complex representations.</param>
/// <param name="dbService">The database persistence service handling direct SQL operations.</param>
public class RepositoryService<TEntity, TComplex>(
    CacheService<TEntity> entityCache,
    CacheService<TComplex> complexCache,
    IDbService<TEntity, TComplex> dbService) : IRepositoryService<TEntity, TComplex> where TEntity : IEntity 
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves all available record IDs directly from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="IdModel"/>s.</returns>
    public Task<List<IdModel>> GetIdsAsync()
    {
        return dbService.GetIdsAsync();
    }

    /// <summary>
    ///     Retrieves all flat entities, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of flat entities.</returns>
    public async Task<List<TEntity>> GetAllSimplesAsync()
    {
        var cacheData = await entityCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllEntityAsync();
        foreach (var item in dbData)
            await entityCache.Set(item);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single flat entity by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the entity if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public async Task<TEntity?> GetSimpleByIdAsync(string id)
    {
        var cacheData = await entityCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByIdAsync(id);
        if (dbData is null) return default;

        await entityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex models, populating the cache on a miss.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of complex models.</returns>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        var cacheData = await complexCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllComplexAsync();
        foreach (var item in dbData)
            await complexCache.Set(item);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single complex model by ID, populating the cache on a miss.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the complex model if found; 
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByIdAsync(id);
        if (dbData is null) return default;

        await complexCache.Set(dbData);
        return dbData;
    }
}
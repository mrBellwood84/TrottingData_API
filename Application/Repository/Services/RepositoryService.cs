using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     A generic repository service that manages data flow between an in-memory cache and the database.
/// </summary>
/// <typeparam name="TSimple">The simple model type representing the entity. Must implement <see cref="IDbItem" />.</typeparam>
/// <typeparam name="TComplex">The complex model type representing the entity. Must implement <see cref="IDbItem" />.</typeparam>
/// <param name="simpleCache">The cache service responsible for storing simple model representations.</param>
/// <param name="complexCache">The cache service responsible for storing complex model representations.</param>
/// <param name="dbService">The database persistence service handling direct SQL operations.</param>
public class RepositoryService<TSimple, TComplex>(
    CacheService<TSimple> simpleCache,
    CacheService<TComplex> complexCache,
    IDbService<TSimple, TComplex> dbService)
    : IRepositoryService<TSimple, TComplex> where TSimple : IDbItem where TComplex : IDbItem
{
    /// <summary>
    ///     Retrieves a list of all available IDs for this entity type.
    /// </summary>
    /// <remarks>
    ///     This query bypasses the cache and queries the database directly,
    ///     as ID listings are lightweight and frequently used for synchronization.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="IdModel" />s.</returns>
    public Task<List<IdModel>> GetIdsAsync()
    {
        return dbService.GetIdsAsync();
    }

    /// <summary>
    ///     Retrieves all simple model representations.
    /// </summary>
    /// <remarks>
    ///     This method checks the cache first (Cache-Aside). If the cache is empty,
    ///     it fetches the data from the database, populates the cache for future requests, and returns the result.
    /// </remarks>
    /// <returns>A list of simple models of type <typeparamref name="TSimple" />.</returns>
    public async Task<List<TSimple>> GetAllSimplesAsync()
    {
        var cacheData = await simpleCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllSimpleAsync();
        foreach (var item in dbData)
            await simpleCache.Set(item.Id, item);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single simple model representation by its unique identifier.
    /// </summary>
    /// <remarks>
    ///     Looks up the item in the cache first. If it is a cache miss, the item is loaded from
    ///     the database, written to the cache, and returned.
    /// </remarks>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The found simple model of type <typeparamref name="TSimple" />, or <see langword="null" /> if not found.</returns>
    public async Task<TSimple?> GetSimpleByIdAsync(string id)
    {
        var cacheData = await simpleCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetSimpleByIdAsync(id);
        if (dbData is null) return default;

        await simpleCache.Set(dbData.Id, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex model representations.
    /// </summary>
    /// <remarks>
    ///     This method checks the complex cache first. If the cache is empty,
    ///     it fetches the complex representations from the database, populates the cache, and returns the list.
    /// </remarks>
    /// <returns>A list of complex models of type <typeparamref name="TComplex" />.</returns>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        var cacheData = await complexCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllComplexAsync();
        foreach (var item in dbData)
            await complexCache.Set(item.Id, item);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single complex model representation by its unique identifier.
    /// </summary>
    /// <remarks>
    ///     Looks up the item in the complex cache first. If it is a cache miss, the complex representation
    ///     is loaded from the database, written to the complex cache, and returned.
    /// </remarks>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The found complex model of type <typeparamref name="TComplex" />, or <see langword="null" /> if not found.</returns>
    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByIdAsync(id);
        if (dbData is null) return default;

        await complexCache.Set(dbData.Id, dbData);
        return dbData;
    }
}
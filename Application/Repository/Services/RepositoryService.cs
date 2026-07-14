using Application.Cache.Services;
using Application.Repository.Exceptions;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Manages data flow between in-memory caches and the database using a Cache-Aside pattern.
///     Enforces configured model policies before performing database operations.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type.</typeparam>
public class RepositoryService<TEntity, TComplex>(
    CacheService<TEntity> entityCache,
    CacheService<TComplex> complexCache,
    IDbService<TEntity, TComplex> dbService,
    ModelPolicy<TEntity> modelPolicy)
    : IRepositoryService<TEntity, TComplex>
    where TEntity : IEntity
    where TComplex : IEntity
{
    private readonly ModelPolicy<TEntity> _modelPolicy = modelPolicy;

    /// <summary>
    ///     Retrieves all available record IDs directly from the database.
    /// </summary>
    /// <returns>A list of available <see cref="IdModel" />s.</returns>
    /// <exception cref="RepositoryPolicyViolationException">Thrown when retrieving IDs is disallowed by active policy.</exception>
    public Task<List<IdModel>> GetIdsAsync()
    {
        if (!_modelPolicy.AllowIdList)
            throw new RepositoryPolicyViolationException(
                $"Retrieving IDs for {typeof(TEntity).Name} is disallowed by policy.");

        return dbService.GetIdsAsync();
    }

    /// <summary>
    ///     Retrieves all flat entities, returning cached data if loaded, or populating the cache on a miss.
    /// </summary>
    /// <returns>A list of flat entities.</returns>
    /// <exception cref="RepositoryPolicyViolationException">Thrown when 'GetAll' is disallowed by active policy.</exception>
    public async Task<List<TEntity>> GetAllEntityAsync()
    {
        if (!_modelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all flat entities for {typeof(TEntity).Name} is disallowed by policy.");

        if (entityCache.Loaded) return await entityCache.GetAll();

        var dbData = await dbService.GetAllEntityAsync();
        await entityCache.Set(dbData);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single flat entity by ID, checking the cache first.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, <see langword="null" />.</returns>
    public async Task<TEntity?> GetEntityByIdAsync(string id)
    {
        var cacheData = await entityCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByIdAsync(id);
        if (dbData is null) return default;

        await entityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex models, returning cached data if loaded, or populating the cache on a miss.
    /// </summary>
    /// <returns>A list of complex models.</returns>
    /// <exception cref="RepositoryPolicyViolationException">Thrown when 'GetAll' is disallowed by active policy.</exception>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        if (!_modelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all complex models for {typeof(TComplex).Name} is disallowed by policy.");

        if (complexCache.Loaded) return await complexCache.GetAll();

        var dbData = await dbService.GetAllComplexAsync();
        await complexCache.Set(dbData);

        return dbData;
    }

    /// <summary>
    ///     Retrieves a single complex model by ID, checking the cache first.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>The complex model if found; otherwise, <see langword="null" />.</returns>
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
using Application.Cache.Services;
using Application.Repository.Exceptions;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Models.Shared;
using Persistence.Services;

namespace Application.Repository.Services;

/// <summary>
///     Provides a generic repository implementation that extends single-item retrieval
///     to support bulk operations for both flat entities and complex domain models,
///     governed by application-defined model policies.
/// </summary>
public class ReadAllRepository<TEntity, TComplex>(
    CacheService<TEntity> entityCache,
    CacheService<TComplex> complexCache,
    IReadAllDbService<TEntity, TComplex> dbService,
    ModelPolicy<TEntity> modelPolicy)
    : ReadSingleRepository<TEntity, TComplex>(entityCache, complexCache, dbService),
        IReadAllRepository<TEntity, TComplex> where TComplex : IEntity
    where TEntity : IEntity
{
    private readonly CacheService<TEntity> _entityCache = entityCache;
    private readonly CacheService<TComplex> _complexCache = complexCache;
    private readonly ModelPolicy<TEntity> _modelPolicy = modelPolicy;

    /// <summary>
    ///     Retrieves a list of all identity models, subject to policy restrictions.
    /// </summary>
    public Task<List<IdModel>> GetAllIdsAsync()
    {
        if (!_modelPolicy.AllowIdList)
            throw new RepositoryPolicyViolationException(
                $"Retrieving IDs for {typeof(TEntity).Name} is disallowed by policy.");

        return dbService.GetIdsAsync();
    }

    /// <summary>
    ///     Retrieves all flat entities, returning the fully loaded cache if available,
    ///     otherwise fetching from the database and populating the cache.
    /// </summary>
    public async Task<List<TEntity>> GetAllEntitiesAsync()
    {
        if (!_modelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all flat entities for {typeof(TEntity).Name} is disallowed by policy.");

        if (_entityCache.Loaded) return await _entityCache.GetAll();

        var dbData = await dbService.GetEntitiesAsync();
        await _entityCache.Set(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex domain models, returning the fully loaded cache if available,
    ///     otherwise fetching from the database and populating the cache.
    /// </summary>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        if (!_modelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all complex models for {typeof(TComplex).Name} is disallowed by policy.");

        if (_complexCache.Loaded) return await _complexCache.GetAll();

        var dbData = await dbService.GetComplexAsync();
        await _complexCache.Set(dbData);
        return dbData;
    }
}
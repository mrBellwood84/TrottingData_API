using Application.Cache.Interfaces;
using Application.Repository.Exceptions;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Provides a generic repository implementation that extends single-item retrieval
///     to support bulk operations for both flat entities and complex domain models,
///     governed by application-defined model policies.
/// </summary>
public class ReadAllRepository<TEntity, TComplex>(
    IReadAllCache<TEntity> entityCache,
    IReadAllCache<TComplex> complexCache,
    IReadAllDbService<TEntity, TComplex> dbService)
    : ReadSingleRepository<TEntity, TComplex>(entityCache, complexCache, dbService),
        IReadAllRepository<TEntity, TComplex> where TEntity : IEntity
    where TComplex : IEntity
{
    protected virtual ModelPolicy ModelPolicy => new() { AllowGetAll = true, AllowIdList = true };

    /// <summary>
    ///     Retrieves a list of all identity models, subject to policy restrictions.
    /// </summary>
    public Task<List<IdModel>> GetAllIdsAsync()
    {
        if (!ModelPolicy.AllowIdList)
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
        if (!ModelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all flat entities for {typeof(TEntity).Name} is disallowed by policy.");

        if (entityCache.Loaded) return await entityCache.GetAllAsync();

        var dbData = await dbService.GetEntitiesAsync();
        await entityCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex domain models, returning the fully loaded cache if available,
    ///     otherwise fetching from the database and populating the cache.
    /// </summary>
    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        if (!ModelPolicy.AllowGetAll)
            throw new RepositoryPolicyViolationException(
                $"Retrieving all complex models for {typeof(TComplex).Name} is disallowed by policy.");

        if (complexCache.Loaded) return await complexCache.GetAllAsync();

        var dbData = await dbService.GetComplexAsync();
        await complexCache.SetAsync(dbData);
        return dbData;
    }
}
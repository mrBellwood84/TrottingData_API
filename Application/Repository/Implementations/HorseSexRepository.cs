using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for horse sexes, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class HorseSexRepository(
    CacheService<HorseSexEntity> entityCache,
    CacheService<HorseSexComplex> complexCache,
    IReadAllDbService<HorseSexEntity, HorseSexComplex> dbService,
    ModelPolicy<HorseSexEntity> modelPolicy)
    : ReadAllRepository<HorseSexEntity, HorseSexComplex>(entityCache, complexCache, dbService, modelPolicy)
{
}
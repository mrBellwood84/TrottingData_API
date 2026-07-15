using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race cart types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceCartTypeRepository(
    CacheService<RaceCartTypeEntity> entityCache,
    CacheService<RaceCartTypeComplex> complexCache,
    IReadAllDbService<RaceCartTypeEntity, RaceCartTypeComplex> dbService)
    : ReadAllRepository<RaceCartTypeEntity, RaceCartTypeComplex>(entityCache, complexCache, dbService)
{
}
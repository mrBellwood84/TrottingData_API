using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race gambling types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceGamblingTypeRepository(
    CacheService<RaceGamblingTypeEntity> entityCache,
    CacheService<RaceGamblingTypeComplex> complexCache,
    IReadAllDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> dbService)
    : ReadAllRepository<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(entityCache, complexCache, dbService)
{
}
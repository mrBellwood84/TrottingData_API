using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race start types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceStartTypeRepository(
    CacheService<RaceStartTypeEntity> entityCache,
    CacheService<RaceStartTypeComplex> complexCache,
    IReadAllDbService<RaceStartTypeEntity, RaceStartTypeComplex> dbService,
    ModelPolicy<RaceStartTypeEntity> modelPolicy)
    : ReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex>(entityCache, complexCache, dbService, modelPolicy)
{
}
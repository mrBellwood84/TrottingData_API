using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for horses, handling specialized caching
///     and lookups for both flat entities and complex domain models using internal
///     identifiers or external source identifiers.
/// </summary>
public sealed class HorseRepository(
    CacheService<HorseEntity> entityCache,
    CacheService<HorseComplex> complexCache,
    SourcedCacheService<HorseEntity> sourcedEntityCache,
    SourcedCacheService<HorseComplex> sourcedComplexCache,
    IReadSourcedDbService<HorseEntity, HorseComplex> dbService)
    : ReadSourcedRepository<HorseEntity, HorseComplex>(
        entityCache,
        complexCache,
        sourcedEntityCache,
        sourcedComplexCache,
        dbService)
{
}
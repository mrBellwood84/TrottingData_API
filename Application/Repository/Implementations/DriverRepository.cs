using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for drivers, handling specialized caching
///     and lookups for both flat entities and complex domain models using internal
///     identifiers or external source identifiers.
/// </summary>
public sealed class DriverRepository(
    CacheService<DriverEntity> entityCache,
    CacheService<DriverComplex> complexCache,
    SourcedCacheService<DriverEntity> sourcedEntityCache,
    SourcedCacheService<DriverComplex> sourcedComplexCache,
    IReadSourcedDbService<DriverEntity, DriverComplex> dbService)
    : ReadSourcedRepository<DriverEntity, DriverComplex>(
        entityCache,
        complexCache,
        sourcedEntityCache,
        sourcedComplexCache,
        dbService)
{
}
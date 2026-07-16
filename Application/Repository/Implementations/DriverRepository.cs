using Application.Cache.Interfaces;
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
    IReadSourceCache<DriverEntity> entityCache,
    IReadSourceCache<DriverComplex> complexCache,
    IReadSourcedDbService<DriverEntity, DriverComplex> dbService)
    : ReadReadSourceRepository<DriverEntity, DriverComplex>(entityCache, complexCache, dbService)
{
}
using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for horses, handling specialized caching
///     and lookups for both flat entities and complex domain models using internal
///     identifiers or external source identifiers.
/// </summary>
public sealed class HorseRepository(
    IReadSourceCache<HorseEntity> entityCache,
    IReadSourceCache<HorseComplex> complexCache,
    IReadSourcedDbService<HorseEntity, HorseComplex> dbService)
    : ReadReadSourceRepository<HorseEntity, HorseComplex>(entityCache, complexCache, dbService)
{
}
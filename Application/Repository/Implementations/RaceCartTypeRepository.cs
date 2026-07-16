using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race cart types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceCartTypeRepository(
    IReadAllCache<RaceCartTypeEntity> entityCache,
    IReadAllCache<RaceCartTypeComplex> complexCache,
    IReadAllDbService<RaceCartTypeEntity, RaceCartTypeComplex> dbService)
    : ReadAllRepository<RaceCartTypeEntity, RaceCartTypeComplex>(entityCache, complexCache, dbService)
{
}
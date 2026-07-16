using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for horse types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class HorseTypeRepository(
    IReadAllCache<HorseTypeEntity> entityCache,
    IReadAllCache<HorseTypeComplex> complexCache,
    IReadAllDbService<HorseTypeEntity, HorseTypeComplex> dbService)
    : ReadAllRepository<HorseTypeEntity, HorseTypeComplex>(entityCache, complexCache, dbService)
{
}
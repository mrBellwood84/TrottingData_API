using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race gambling types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceGamblingTypeRepository(
    IListItemCache<RaceGamblingTypeEntity> entityCache,
    IListItemCache<RaceGamblingTypeComplex> complexCache,
    IReadAllDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> dbService)
    : ListItemsRepository<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(entityCache, complexCache, dbService)
{
}
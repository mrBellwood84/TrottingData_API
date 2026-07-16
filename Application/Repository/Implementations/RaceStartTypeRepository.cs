using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race start types, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceStartTypeRepository(
    IListItemCache<RaceStartTypeEntity> entityCache,
    IListItemCache<RaceStartTypeComplex> complexCache,
    IReadAllDbService<RaceStartTypeEntity, RaceStartTypeComplex> dbService)
    : ListItemsRepository<RaceStartTypeEntity, RaceStartTypeComplex>(entityCache, complexCache, dbService)
{
}
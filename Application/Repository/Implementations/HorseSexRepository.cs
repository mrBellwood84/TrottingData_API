using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for horse sexes, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class HorseSexRepository(
    IListItemCache<HorseSexEntity> entityCache,
    IListItemCache<HorseSexComplex> complexCache,
    IReadAllDbService<HorseSexEntity, HorseSexComplex> dbService)
    : ListItemsRepository<HorseSexEntity, HorseSexComplex>(entityCache, complexCache, dbService)
{
}
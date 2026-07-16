using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for race courses, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class RaceCourseRepository(
    IListItemCache<RaceCourseEntity> entityCache,
    IListItemCache<RaceCourseComplex> complexCache,
    IReadAllDbService<RaceCourseEntity, RaceCourseComplex> dbService)
    : ListItemsRepository<RaceCourseEntity, RaceCourseComplex>(entityCache, complexCache, dbService)
{
}
using Application.Cache.Services;
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
    CacheService<RaceCourseEntity> entityCache,
    CacheService<RaceCourseComplex> complexCache,
    IReadAllDbService<RaceCourseEntity, RaceCourseComplex> dbService)
    : ReadAllRepository<RaceCourseEntity, RaceCourseComplex>(entityCache, complexCache, dbService)
{
}
using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;
using Persistence.Services;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for driver licenses, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class DriverLicenseRepository(
    CacheService<DriverLicenseEntity> entityCache,
    CacheService<DriverLicenseComplex> complexCache,
    IReadAllDbService<DriverLicenseEntity, DriverLicenseComplex> dbService)
    : ReadAllRepository<DriverLicenseEntity, DriverLicenseComplex>(entityCache, complexCache, dbService)
{
}
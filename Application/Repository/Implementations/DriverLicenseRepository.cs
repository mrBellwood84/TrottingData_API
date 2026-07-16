using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Provides repository operations for driver licenses, handling caching and policy
///     checks for both flat entities and complex domain models.
/// </summary>
public sealed class DriverLicenseRepository(
    IReadAllCache<DriverLicenseEntity> entityCache,
    IReadAllCache<DriverLicenseComplex> complexCache,
    IReadAllDbService<DriverLicenseEntity, DriverLicenseComplex> dbService)
    : ReadAllRepository<DriverLicenseEntity, DriverLicenseComplex>(entityCache, complexCache, dbService)
{
}
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
    IListItemCache<DriverLicenseEntity> entityCache,
    IListItemCache<DriverLicenseComplex> complexCache,
    IReadAllDbService<DriverLicenseEntity, DriverLicenseComplex> dbService)
    : ListItemsRepository<DriverLicenseEntity, DriverLicenseComplex>(entityCache, complexCache, dbService)
{
}
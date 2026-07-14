using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

/// <inheritdoc />
public class DriverLicenseController(
    IRepositoryService<DriverLicenseEntity, DriverLicenseComplex> repository)
    : ModelController<DriverLicenseEntity, DriverLicenseComplex>(repository)
{
}
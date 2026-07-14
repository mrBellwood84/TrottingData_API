using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class DriverLicenseController(
    IRepositoryService<DriverLicenseEntity, DriverLicenseComplex> repository,
    ModelPolicy<DriverLicenseEntity> modelPolicy)
    : ModelController<DriverLicenseEntity, DriverLicenseComplex>(repository, modelPolicy)
{
}
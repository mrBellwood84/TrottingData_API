using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class RaceGamblingTypeController(
    IRepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> repository,
    ModelPolicy<RaceGamblingTypeEntity> modelPolicy)
    : ModelController<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(repository, modelPolicy)
{
}
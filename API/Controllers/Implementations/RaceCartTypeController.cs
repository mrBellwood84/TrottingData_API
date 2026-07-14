using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class RaceCartTypeController(
    IRepositoryService<RaceCartTypeEntity, RaceCartTypeComplex> repository,
    ModelPolicy<RaceCartTypeEntity> modelPolicy)
    : ModelController<RaceCartTypeEntity, RaceCartTypeComplex>(repository, modelPolicy)
{
}
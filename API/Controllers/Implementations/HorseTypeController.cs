using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class HorseTypeController(
    IRepositoryService<HorseTypeEntity, HorseTypeComplex> repository,
    ModelPolicy<HorseTypeEntity> modelPolicy)
    : ModelController<HorseTypeEntity, HorseTypeComplex>(repository, modelPolicy)
{
}
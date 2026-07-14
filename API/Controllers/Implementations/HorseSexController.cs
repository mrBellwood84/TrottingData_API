using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class HorseSexController(
    IRepositoryService<HorseSexEntity, HorseSexComplex> repository,
    ModelPolicy<HorseSexEntity> modelPolicy)
    : ModelController<HorseSexEntity, HorseSexComplex>(repository, modelPolicy)
{
}
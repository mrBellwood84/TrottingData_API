using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class RaceStartTypeController(
    IRepositoryService<RaceStartTypeEntity, RaceStartTypeComplex> repository,
    ModelPolicy<RaceStartTypeEntity> modelPolicy)
    : ModelController<RaceStartTypeEntity, RaceStartTypeComplex>(repository, modelPolicy)
{
}
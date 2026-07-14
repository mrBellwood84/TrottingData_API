using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entities;
using Models.Simple;

namespace API.Controllers.Implementations;

public class RaceCourseController(
    IRepositoryService<RaceCourseEntity, RaceCourseComplex> repository,
    ModelPolicy<RaceCourseEntity> modelPolicy)
    : ModelController<RaceCourseEntity, RaceCourseComplex>(repository, modelPolicy)
{
}
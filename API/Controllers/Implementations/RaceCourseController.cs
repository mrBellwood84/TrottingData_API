using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

public class RaceCourseController(
    IRepositoryService<RaceCourseEntity, RaceCourseComplex> repository)
    : ModelController<RaceCourseEntity, RaceCourseComplex>(repository)
{
}
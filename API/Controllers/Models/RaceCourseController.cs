using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceCourseController(IRepositoryService<RaceCourseEntity, RaceCourseComplex> repository)
    : ReadFullModelController<RaceCourseEntity, RaceCourseComplex>(repository)
{
}
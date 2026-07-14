using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

/// <inheritdoc />
public class HorseTypeController(
    IRepositoryService<HorseTypeEntity, HorseTypeComplex> repository)
    : ModelController<HorseTypeEntity, HorseTypeComplex>(repository)
{
}
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

/// <inheritdoc />
public class HorseSexController(
    IRepositoryService<HorseSexEntity, HorseSexComplex> repository)
    : ModelController<HorseSexEntity, HorseSexComplex>(repository)
{
}
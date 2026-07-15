using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class HorseSexController(IRepositoryService<HorseSexEntity, HorseSexComplex> repository)
    : ReadFullModelController<HorseSexEntity, HorseSexComplex>(repository)
{
}
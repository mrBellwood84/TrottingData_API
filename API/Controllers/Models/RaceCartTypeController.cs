using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

/// <summary>
///     Provides read-only endpoints for managing and retrieving race cart type data,
///     supporting identity lookups, flat entity lists, and complex domain models.
/// </summary>
public class RaceCartTypeController(IListItemsRepository<RaceCartTypeEntity, RaceCartTypeComplex> repository)
    : ReadAllModelController<RaceCartTypeEntity, RaceCartTypeComplex>(repository)
{
}
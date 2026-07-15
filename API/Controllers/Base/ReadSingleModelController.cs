using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;

namespace API.Controllers.Base;

/// <summary>
///     A base controller providing read-only endpoints for retrieving single instances of flat entities and complex models by their unique identifier.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type, implementing <see cref="IEntity"/>.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type, implementing <see cref="IEntity"/>.</typeparam>
/// <param name="repository">The repository service responsible for data retrieval and policy enforcement.</param>
[ApiController]
[Route("model/[controller]")]
[Produces("application/json")]
public class ReadSingleModelController<TEntity, TComplex>(IReadSingleRepository<TEntity, TComplex> repository)
    : ControllerBase
    where TEntity : IEntity
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a single flat entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The flat entity if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested flat entity.</response>
    /// <response code="404">If no entity with the specified ID exists.</response>
    [HttpGet("entity/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TEntity>> GetEntityByIdAsync([FromRoute] string id)
    {
        var data = await repository.GetEntityByIdAsync(id);
        if (data is null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves a single complex model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the complex model.</param>
    /// <returns>The complex model if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested complex model.</response>
    /// <response code="404">If no complex model with the specified ID exists.</response>
    [HttpGet("complex/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TComplex>> GetComplexByIdAsync([FromRoute] string id)
    {
        var data = await repository.GetComplexByIdAsync(id);
        if (data is null) return NotFound();
        return Ok(data);
    }
}
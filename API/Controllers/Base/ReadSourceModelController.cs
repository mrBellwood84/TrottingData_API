using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;

namespace API.Controllers.Base;

/// <summary>
///     An extended base controller providing read-only endpoints for retrieving single instances
///     of flat entities and complex models using their external Source ID.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type. Must implement <see cref="ISourcedEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type. Must implement <see cref="ISourcedEntity" />.</typeparam>
/// <param name="repository">The specialized repository service handling sourced database operations.</param>
public class ReadSourceModelController<TEntity, TComplex>(ISourcedRepositoryService<TEntity, TComplex> repository)
    : ReadSingleModelController<TEntity, TComplex>(repository)
    where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    private readonly ISourcedRepositoryService<TEntity, TComplex> _repository = repository;

    /// <summary>
    ///     Retrieves a single flat entity by its external Source ID.
    /// </summary>
    /// <param name="sourceId">The external Source ID of the entity.</param>
    /// <returns>The flat entity if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested flat entity.</response>
    /// <response code="404">If no entity with the specified Source ID exists.</response>
    [HttpGet("entity/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TEntity>> GetEntityBySourceIdAsync([FromRoute] string sourceId)
    {
        var data = await _repository.GetEntityBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves a single complex model by its external Source ID.
    /// </summary>
    /// <param name="sourceId">The external Source ID of the complex model.</param>
    /// <returns>The complex model if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested complex model.</response>
    /// <response code="404">If no complex model with the specified Source ID exists.</response>
    [HttpGet("complex/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TComplex>> GetComplexBySourceIdAsync([FromRoute] string sourceId)
    {
        var data = await _repository.GetComplexBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        return Ok(data);
    }
}
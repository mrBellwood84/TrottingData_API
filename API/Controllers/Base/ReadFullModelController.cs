using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Models.Shared;

namespace API.Controllers.Base;

/// <summary>
///     An extended base controller providing read-only endpoints for retrieving both single instances
///     and complete collections (IDs, flat entities, and complex models) of data.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type.</typeparam>
/// <param name="repository">The repository service responsible for data flow and policy enforcement.</param>
public class ReadFullModelController<TEntity, TComplex>(IRepositoryService<TEntity, TComplex> repository)
    : ReadSingleModelController<TEntity, TComplex>(repository)
    where TEntity : IEntity
    where TComplex : IEntity
{
    private readonly IRepositoryService<TEntity, TComplex> _repository = repository;

    /// <summary>
    ///     Retrieves a list of all available record IDs.
    /// </summary>
    /// <returns>A list of available <see cref="IdModel" />s.</returns>
    /// <response code="200">Returns the list of available record IDs.</response>
    /// <response code="403">
    ///     Forbidden if retrieving the ID list is restricted by active model policies (exceptional cases
    ///     only).
    /// </response>
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<IdModel>>> GetIdListAsync()
    {
        var data = await _repository.GetIdsAsync();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all flat entities from the cache or database.
    /// </summary>
    /// <returns>A list of all flat entities.</returns>
    /// <response code="200">Returns the list of flat entities.</response>
    /// <response code="403">
    ///     Forbidden if bulk retrieval of flat entities is restricted by active model policies (exceptional
    ///     cases only).
    /// </response>
    [HttpGet("entity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<TEntity>>> GetEntityListAsync()
    {
        var data = await _repository.GetAllEntityAsync();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex models from the cache or database.
    /// </summary>
    /// <returns>A list of all complex models.</returns>
    /// <response code="200">Returns the list of complex models.</response>
    /// <response code="403">
    ///     Forbidden if bulk retrieval of complex models is restricted by active model policies (exceptional
    ///     cases only).
    /// </response>
    [HttpGet("complex")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<TComplex>>> GetAllComplex()
    {
        var data = await _repository.GetAllComplexAsync();
        return Ok(data);
    }
}
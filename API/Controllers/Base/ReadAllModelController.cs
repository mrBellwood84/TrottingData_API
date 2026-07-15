using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Models.Shared;

namespace API.Controllers.Base;

/// <summary>
///     A base controller providing read-only endpoints for retrieving collections of identity models,
///     flat entities, and complex models.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type, implementing <see cref="IEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type, implementing <see cref="IEntity" />.</typeparam>
/// <param name="repository">The bulk read repository service responsible for data flow and policy enforcement.</param>
public class ReadAllModelController<TEntity, TComplex>(IReadAllRepository<TEntity, TComplex> repository)
    : ReadSingleModelController<TEntity, TComplex>(repository)
    where TEntity : IEntity
    where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a list of all identity models, containing basic ID and identification data.
    /// </summary>
    /// <returns>A list of identity models.</returns>
    /// <response code="200">Returns the list of identity models.</response>
    /// <response code="403">If the request fails policy restrictions or authorization checks for listing identifiers.</response>
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<IdModel>>> GetIdListAsync()
    {
        var data = await repository.GetAllIdsAsync();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all flat entities, utilizing the fully loaded cache if available.
    /// </summary>
    /// <returns>A list of all flat entities.</returns>
    /// <response code="200">Returns the requested list of flat entities.</response>
    /// <response code="403">If the request fails policy restrictions or authorization checks for bulk entity retrieval.</response>
    [HttpGet("entity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<TEntity>>> GetEntityListAsync()
    {
        var data = await repository.GetAllEntitiesAsync();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex domain models, utilizing the fully loaded cache if available.
    /// </summary>
    /// <returns>A list of all complex domain models.</returns>
    /// <response code="200">Returns the requested list of complex models.</response>
    /// <response code="403">If the request fails policy restrictions or authorization checks for bulk complex model retrieval.</response>
    [HttpGet("complex")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<TComplex>>> GetAllComplexAsync()
    {
        var data = await repository.GetAllComplexAsync();
        return Ok(data);
    }
}
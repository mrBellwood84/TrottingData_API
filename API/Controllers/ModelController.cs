using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("/model/[controller]")]
public class ModelController<TSimple, TComplex>(
    IRepositoryService<TSimple, TComplex> repository,
    ModelPolicy<TSimple> modelPolicy)
    : ControllerBase where TSimple : IDbItem where TComplex : IDbItem
{
    [HttpGet("id")]
    public async Task<ActionResult<List<IdModel>>> GetIdsAsync()
    {
        var data = await repository.GetIdsAsync();
        return Ok(data);
    }

    [HttpGet("entity")]
    public async Task<ActionResult<List<TSimple>>> GetEntitiesAsync()
    {
        if (!modelPolicy.AllowGetAll)
            return StatusCode(StatusCodes.Status405MethodNotAllowed,
                $"Bulk retrieval of {typeof(TSimple).Name} is disabled due to dataset size.");

        var data = await repository.GetAllSimplesAsync();
        return Ok(data);
    }

    [HttpGet("entity/{id}")]
    public async Task<ActionResult<TSimple>> GetEntityAsync(string id)
    {
        var data = await repository.GetSimpleByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpGet("complex")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntitiesAsync()
    {
        if (!modelPolicy.AllowGetAll)
            return StatusCode(StatusCodes.Status405MethodNotAllowed,
                $"Bulk retrieval of complex {typeof(TComplex).Name} is disabled due to dataset size.");

        var data = await repository.GetAllComplexAsync();
        return Ok(data);
    }

    [HttpGet("complex/{id}")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntityAsync(string id)
    {
        var data = await repository.GetComplexByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }
}
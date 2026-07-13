using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Persistence.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("/model/[controller]")]
public class ModelController<TSimple, TComplex>(IDbService<TSimple, TComplex> dbService) : ControllerBase
{
    [HttpGet("id")]
    public async Task<ActionResult<List<IdEntity>>> GetIdsAsync()
    {
        var data = await dbService.GetIdsAsync();
        return Ok(data);
    }

    [HttpGet("entity")]
    public async Task<ActionResult<List<TSimple>>> GetEntitiesAsync()
    {
        var data = await dbService.GetAllSimpleAsync();
        return Ok(data);
    }

    [HttpGet("entity/{id}")]
    public async Task<ActionResult<IdEntity>> GetEntityAsync(string id)
    {
        var data = await dbService.GetSimpleByIdAsync(id);
        return Ok(data);
    }

    [HttpGet("complex")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntitiesAsync()
    {
        var data = await dbService.GetAllComplexAsync();
        return Ok(data);
    }

    [HttpGet("/complex/{id}")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntityAsync(string id)
    {
        var data = await dbService.GetComplexByIdAsync(id);
        return Ok(data);
    }
}
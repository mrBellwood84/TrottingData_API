using API.Controllers.Base;
using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceController(IRaceRepository repository)
    : ReadSingleModelController<RaceEntity, RaceComplex>(repository)
{
    [HttpGet("entity/competition/{competitionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceEntity>>> GetRaceEntitiesByCompetitionId([FromRoute] string competitionId)
    {
        var data = await repository.GetRaceEntitiesByCompetitionIdAsync(competitionId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpGet("complex/competition/{competitionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceComplex>>> GetRaceComplexByCompetitionId([FromRoute] string competitionId)
    {
        var data = await repository.GetRaceComplexByCompetitionIdAsync(competitionId);
        if (data == null) return NotFound();
        return Ok(data);
    }
}
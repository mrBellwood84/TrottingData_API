using API.Controllers.Base;
using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

/// <summary>
///     API controller managing read operations for race participants.
///     Provides endpoints to query participants sliced by races, drivers, horses, and trainers.
/// </summary>
public class RaceParticipantController(IRaceParticipantRepository repository)
    : ReadSingleModelController<RaceParticipantEntity, RaceParticipantComplex>(repository)
{
    /// <summary>
    ///     Retrieves all flat participant entities registered to a specific race.
    /// </summary>
    [HttpGet("entity/race/{raceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantEntity>>> GetParticipantEntitiesByRaceAsync(
        [FromRoute] string raceId)
    {
        var data = await repository.GetEntitiesByRaceIdAsync(raceId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex participant models registered to a specific race.
    /// </summary>
    [HttpGet("complex/race/{raceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantComplex>>> GetParticipantComplexByRaceAsync(
        [FromRoute] string raceId)
    {
        var data = await repository.GetComplexByRaceIdAsync(raceId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific driver.
    /// </summary>
    [HttpGet("entity/driver/{driverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantEntity>>> GetParticipantEntitiesByDriverAsync(
        [FromRoute] string driverId)
    {
        var data = await repository.GetEntitiesByDriverAsync(driverId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific driver.
    /// </summary>
    [HttpGet("complex/driver/{driverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantComplex>>> GetParticipantComplexByDriverAsync(
        [FromRoute] string driverId)
    {
        var data = await repository.GetComplexByDriverAsync(driverId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific horse.
    /// </summary>
    [HttpGet("entity/horse/{horseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantEntity>>> GetParticipantEntitiesByHorseAsync(
        [FromRoute] string horseId)
    {
        var data = await repository.GetEntitiesByHorseAsync(horseId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific horse.
    /// </summary>
    [HttpGet("complex/horse/{horseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantComplex>>> GetParticipantComplexByHorseAsync(
        [FromRoute] string horseId)
    {
        var data = await repository.GetComplexesByHorseAsync(horseId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific trainer.
    /// </summary>
    [HttpGet("entity/trainer/{trainerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantEntity>>> GetParticipantEntitiesByTrainerAsync(
        [FromRoute] string trainerId)
    {
        var data = await repository.GetEntitiesByTrainerAsync(trainerId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific trainer.
    /// </summary>
    [HttpGet("complex/trainer/{trainerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RaceParticipantComplex>>> GetParticipantComplexByTrainerAsync(
        [FromRoute] string trainerId)
    {
        var data = await repository.GetComplexesByTrainerAsync(trainerId);
        if (data == null) return NotFound();
        return Ok(data);
    }
}
using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race participants.
///     Inherits read-only capabilities from ReadSingleDbService and implements
///     specialized queries for both flat entities and complex nested structures.
/// </summary>
public sealed class RaceParticipantDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceParticipantEntity, RaceParticipantComplex>(configuration), IRaceParticipantDbService
{
    // Query to retrieve flat entities belonging to a specific race
    private const string SqlSelectEntitiesByRaceId =
        "SELECT * FROM RaceParticipant WHERE RaceId = @RaceId";

    // Query to retrieve flat entities belonging to a specific driver source ID
    private const string SqlSelectEntitiesByDriverSourceId =
        "SELECT * FROM RaceParticipant WHERE DriverSourceId = @DriverSourceId";

    // Query to retrieve flat entities belonging to a specific horse source ID
    private const string SqlSelectEntitiesByHorseSourceId =
        "SELECT * FROM RaceParticipant WHERE HorseSourceId = @HorseSourceId";

    // Query to retrieve flat entities belonging to a specific trainer source ID
    private const string SqlSelectEntitiesByTrainerSourceId =
        "SELECT * FROM RaceParticipant WHERE TrainerSourceId = @TrainerSourceId";

    /// <summary>
    ///     Base query to fetch complex participant objects.
    ///     Updated to select and join Trainer (t.*) as well.
    /// </summary>
    private const string SqlSelectComplexBase = @"
        SELECT 
            rp.*, 
            d.*, 
            h.*, 
            t.*, 
            ct.*, 
            rr.*
        FROM RaceParticipant rp
        LEFT JOIN Driver d ON rp.DriverSourceId = d.SourceId
        LEFT JOIN Horse h ON rp.HorseSourceId = h.SourceId
        LEFT JOIN Driver t ON rp.TrainerSourceId = t.SourceId
        LEFT JOIN RaceCartType ct ON rp.CartTypeId = ct.Id
        LEFT JOIN RaceResults rr ON rp.Id = rr.RaceParticipantId";

    // Simple query to retrieve a flat participant entity by its primary ID
    protected override string SqlSelectEntityById =>
        "SELECT * FROM RaceParticipant WHERE Id = @Id";

    // Reuses the base query to retrieve a specific complex participant by ID
    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE rp.Id = @Id";

    /// <summary>
    ///     Retrieves flat participant entities belonging to a specific race.
    /// </summary>
    public Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByRaceIdAsync(string raceId)
    {
        return QueryEntityListAsync(SqlSelectEntitiesByRaceId, new { RaceId = raceId });
    }

    /// <summary>
    ///     Retrieves complex participant models belonging to a specific race.
    /// </summary>
    public Task<IEnumerable<RaceParticipantComplex>> GetComplexesByRaceIdAsync(string raceId)
    {
        return QueryComplexListInternalAsync($"{SqlSelectComplexBase} WHERE rp.RaceId = @RaceId",
            new { RaceId = raceId });
    }

    /// <summary>
    ///     Retrieves flat participant entities associated with a driver's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByDriverSourceIdAsync(string driverSourceId)
    {
        return QueryEntityListAsync(SqlSelectEntitiesByDriverSourceId, new { DriverSourceId = driverSourceId });
    }

    /// <summary>
    ///     Retrieves complex participant models associated with a driver's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantComplex>> GetComplexesByDriverSourceIdAsync(string driverSourceId)
    {
        return QueryComplexListInternalAsync($"{SqlSelectComplexBase} WHERE rp.DriverSourceId = @DriverSourceId",
            new { DriverSourceId = driverSourceId });
    }

    /// <summary>
    ///     Retrieves flat participant entities associated with a horse's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByHorseSourceIdAsync(string horseSourceId)
    {
        return QueryEntityListAsync(SqlSelectEntitiesByHorseSourceId, new { HorseSourceId = horseSourceId });
    }

    /// <summary>
    ///     Retrieves complex participant models associated with a horse's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantComplex>> GetComplexesByHorseSourceIdAsync(string horseSourceId)
    {
        return QueryComplexListInternalAsync($"{SqlSelectComplexBase} WHERE rp.HorseSourceId = @HorseSourceId",
            new { HorseSourceId = horseSourceId });
    }

    /// <summary>
    ///     Retrieves flat participant entities associated with a trainer's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByTrainerSourceIdAsync(string trainerSourceId)
    {
        return QueryEntityListAsync(SqlSelectEntitiesByTrainerSourceId, new { TrainerSourceId = trainerSourceId });
    }

    /// <summary>
    ///     Retrieves complex participant models associated with a trainer's source ID.
    /// </summary>
    public Task<IEnumerable<RaceParticipantComplex>> GetComplexesByTrainerSourceIdAsync(string trainerSourceId)
    {
        return QueryComplexListInternalAsync($"{SqlSelectComplexBase} WHERE rp.TrainerSourceId = @TrainerSourceId",
            new { TrainerSourceId = trainerSourceId });
    }

    /// <summary>
    ///     Helper method for ReadSingleDbService to retrieve and return the first match of a complex model.
    /// </summary>
    protected override async Task<RaceParticipantComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    /// <summary>
    ///     Helper method to retrieve a list of simple, flat participant entities.
    /// </summary>
    private async Task<IEnumerable<RaceParticipantEntity>> QueryEntityListAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();
        return await connection.QueryAsync<RaceParticipantEntity>(sql, param);
    }

    /// <summary>
    ///     Executes the multi-mapping SQL query to hydrate nested Driver, Horse, Trainer,
    ///     CartType, and RaceResults complexes into a newly instanced RaceParticipantComplex.
    /// </summary>
    private async Task<IEnumerable<RaceParticipantComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        // Mapped to match: rp.*, d.*, h.*, t.*, ct.*, rr.*
        return await connection.QueryAsync<
            RaceParticipantComplex,
            DriverComplex,
            HorseComplex,
            DriverComplex, // Added Trainer (t.*)
            RaceCartTypeComplex,
            RaceResultsComplex,
            RaceParticipantComplex>(
            sql,
            (participant, driver, horse, trainer, cartType, result) => new RaceParticipantComplex
            {
                Id = participant.Id,
                TrainerSourceId = participant.TrainerSourceId,
                StartNumber = participant.StartNumber,
                TrackNumber = participant.TrackNumber,
                TrackDistance = participant.TrackDistance,
                ForeShoe = participant.ForeShoe,
                HindShoe = participant.HindShoe,
                Driver = driver,
                Horse = horse,
                Trainer = trainer, // Properly assigned to the new Trainer property!
                CartType = cartType,
                Result = result
            },
            param,
            splitOn: "Id"); // Automatically splits on every "Id" column across the selected tables
    }
}
using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race participants.
///     Manages operations for both simple flat database entities and deep, complex models
///     by hydrating related records (Drivers, Horses, Trainers, Carts, and Results) via Dapper multi-mapping.
/// </summary>
public class RaceParticipantDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceParticipantEntity, RaceParticipantComplex>(configuration), IRaceParticipantDbService
{
    // todo : add depth to driver and horse entities
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

    private readonly string _sqlSelectEntityBase = "SELECT * FROM RaceParticipant";

    protected override string SqlSelectEntityById => $"{_sqlSelectEntityBase} WHERE Id = @Id";
    private string SqlSelectEntityByRaceId => $"{_sqlSelectEntityBase} WHERE RaceId = @Id";
    private string SqlSelectEntityByDriverSourceId => $"{_sqlSelectEntityBase} WHERE DriverSourceId = @Id";
    private string SqlSelectEntityByHorseSourceId => $"{_sqlSelectEntityBase} WHERE HorseSourceId = @Id";
    private string SqlSelectEntityByTrainerSourceId => $"{_sqlSelectEntityBase} WHERE TrainerSourceId = @Id";

    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE rp.Id = @Id";
    private string SqlSelectComplexByRaceId => $"{SqlSelectComplexBase} WHERE rp.RaceId = @Id";
    private string SqlSelectComplexByDriverSourceId => $"{SqlSelectComplexBase} WHERE rp.DriverSourceId = @Id";
    private string SqlSelectComplexByHorseSourceId => $"{SqlSelectComplexBase} WHERE rp.HorseSourceId = @Id";
    private string SqlSelectComplexByTrainerSourceId => $"{SqlSelectComplexBase} WHERE rp.TrainerSourceId = @Id";

    /// <summary>
    ///     Retrieves a list of flat participant entities registered for a specific race.
    /// </summary>
    public Task<List<RaceParticipantEntity>> GetEntitiesByRaceIdAsync(string raceId)
    {
        return QueryEntityListInternalAsync(SqlSelectEntityByRaceId, new { Id = raceId });
    }

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific driver source ID.
    /// </summary>
    public Task<List<RaceParticipantEntity>> GetEntitiesByDriverSourceIdAsync(string driverSourceId)
    {
        return QueryEntityListInternalAsync(SqlSelectEntityByDriverSourceId, new { Id = driverSourceId });
    }

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific horse source ID.
    /// </summary>
    public Task<List<RaceParticipantEntity>> GetEntitiesByHorseSourceIdAsync(string horseSourceId)
    {
        return QueryEntityListInternalAsync(SqlSelectEntityByHorseSourceId, new { Id = horseSourceId });
    }

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific trainer source ID.
    /// </summary>
    public Task<List<RaceParticipantEntity>> GetEntitiesByTrainerSourceIdAsync(string trainerSourceId)
    {
        return QueryEntityListInternalAsync(SqlSelectEntityByTrainerSourceId, new { Id = trainerSourceId });
    }

    /// <summary>
    ///     Retrieves all participants for a specific race, fully hydrated with drivers, horses, trainers, and results.
    /// </summary>
    public Task<List<RaceParticipantComplex>> GetComplexesByRaceIdAsync(string raceId)
    {
        return QueryComplexListInternalAsync(SqlSelectComplexByRaceId, new { Id = raceId });
    }

    /// <summary>
    ///     Retrieves all race appearances for a driver, fully hydrated with related domain complexes.
    /// </summary>
    public Task<List<RaceParticipantComplex>> GetComplexesByDriverSourceIdAsync(string driverSourceId)
    {
        return QueryComplexListInternalAsync(SqlSelectComplexByDriverSourceId, new { Id = driverSourceId });
    }

    /// <summary>
    ///     Retrieves all race appearances for a specific horse, fully hydrated with related domain complexes.
    /// </summary>
    public Task<List<RaceParticipantComplex>> GetComplexesByHorseSourceIdAsync(string horseSourceId)
    {
        return QueryComplexListInternalAsync(SqlSelectComplexByHorseSourceId, new { Id = horseSourceId });
    }

    /// <summary>
    ///     Retrieves all race entries managed by a specific trainer, fully hydrated with related domain complexes.
    /// </summary>
    public Task<List<RaceParticipantComplex>> GetComplexesByTrainerSourceIdAsync(string trainerSourceId)
    {
        return QueryComplexListInternalAsync(SqlSelectComplexByTrainerSourceId, new { Id = trainerSourceId });
    }

    /// <summary>
    ///     Overrides the base method to reuse the unified multi-mapping pipeline for a single identifier lookup.
    /// </summary>
    protected override async Task<RaceParticipantComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    /// <summary>
    ///     Executes flat table reads for baseline entity lists.
    /// </summary>
    private async Task<List<RaceParticipantEntity>> QueryEntityListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceParticipantEntity>(sql, param);
        return data.ToList();
    }

    /// <summary>
    ///     Performs multi-mapping over the tabular result rows. Maps via the flat database representation
    ///     into the targeted rich domain complexes to ensure predictable object lifecycle binding.
    /// </summary>
    private async Task<List<RaceParticipantComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        // Performs a single-pass hydration across 6 mapping boundaries.
        // Dapper seamlessly splits on every new encounter of the 'Id' column.
        var rows = await connection.QueryAsync<
            RaceParticipantEntity,
            DriverComplex,
            HorseComplex,
            DriverComplex, // Maps Driver twin alias (t.*) to Trainer
            RaceCartTypeComplex,
            RaceResultsComplex,
            RaceParticipantComplex>(
            sql,
            (entity, driver, horse, trainer, cartType, result) => new RaceParticipantComplex
            {
                Id = entity.Id,
                TrainerSourceId = entity.TrainerSourceId,
                StartNumber = entity.StartNumber,
                TrackNumber = entity.TrackNumber,
                TrackDistance = entity.TrackDistance,
                ForeShoe = entity.ForeShoe,
                HindShoe = entity.HindShoe,
                Driver = driver,
                Horse = horse,
                Trainer = trainer,
                CartType = cartType,
                Result = result
            },
            param,
            splitOn: "Id");

        return rows.ToList();
    }
}
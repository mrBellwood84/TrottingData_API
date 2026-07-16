using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race participants.
///     Inherits read-only capabilities from ReadSingleDbService and implements
///     specialized queries for both flat entities and complex nested structures.
/// </summary>
public sealed class RaceParticipantDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceParticipantEntity, RaceParticipantComplex>(configuration)
{
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

    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE Id = @Id";
    private string SqlSelectComplexByRaceId => $"{SqlSelectComplexBase} WHERE RaceId = @Id";
    private string SqlSelectComplexByDriverSourceId => $"{SqlSelectComplexBase} WHERE DriverSourceId = @Id";
    private string SqlSelectComplexByHorseSourceId => $"{SqlSelectComplexBase} WHERE HorseSourceId = @Id";
    private string SqlSelectComplexByTrainerSourceId => $"{SqlSelectComplexBase} WHERE TrainerSourceId = @Id";


    public Task<RaceParticipantEntity?> GetEntityByRaceId(string raceId)
    {
        var param = new { Id = raceId };
        return QueryEntityAsync(SqlSelectEntityByRaceId, param);
    }

    public Task<RaceParticipantEntity?> GetEntityByDriverSourceId(string driverSourceId)
    {
        var param = new { Id = driverSourceId };
        return QueryEntityAsync(SqlSelectEntityByDriverSourceId, param);
    }

    public Task<RaceParticipantEntity?> GetEntityByHorseSourceId(string horseSourceId)
    {
        var param = new { Id = horseSourceId };
        return QueryEntityAsync(SqlSelectEntityByHorseSourceId, param);
    }

    public Task<RaceParticipantEntity?> GetEntityByTrainerSourceId(string trainerSourceId)
    {
        var param = new { Id = trainerSourceId };
        return QueryEntityAsync(SqlSelectEntityByTrainerSourceId, param);
    }

    
    public Task<RaceParticipantComplex?> GetComplexByRaceId(string raceId)
    {
        var param = new { Id = raceId };
        return QueryComplexAsync(SqlSelectComplexByRaceId, param);
    }

    public Task<RaceParticipantComplex?> GetComplexByDriverSourceId(string driverSourceId)
    {
        var param = new { Id = driverSourceId };
        return QueryComplexAsync(SqlSelectComplexByDriverSourceId, param);
    }

    public Task<RaceParticipantComplex?> GetComplexByHorseSourceId(string horseSourceId)
    {
        var param = new { Id = horseSourceId };
        return QueryComplexAsync(SqlSelectComplexByHorseSourceId, param);
    }

    public Task<RaceParticipantComplex?> GetComplexByTrainerSourceId(string trainerSourceId)
    {
        var param = new { Id = trainerSourceId };
        return QueryComplexAsync(SqlSelectComplexByTrainerSourceId, param);
    }


    protected override async Task<RaceParticipantComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    private async Task<IEnumerable<RaceParticipantComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        return await connection.QueryAsync<
            RaceParticipantComplex,
            DriverComplex,
            HorseComplex,
            DriverComplex,
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
                Trainer = trainer,
                CartType = cartType,
                Result = result
            },
            param,
            splitOn: "Id");
    }
}
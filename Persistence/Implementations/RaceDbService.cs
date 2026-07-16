using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race data.
///     Manages queries for both flat race entities and deeply nested complex race structures,
///     resolving multi-table relations in-memory to prevent cartesian explosion.
/// </summary>
public class RaceDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceEntity, RaceComplex>(configuration), IRaceDbService
{
    // todo : add depth to driver and horse entity
    private const string SqlSelectComplexBase = @"
        SELECT 
            r.*, 
            ht.*, 
            rst.*, 
            rp.*, 
            d.*, 
            h.*, 
            t.*,
            ct.*, 
            rr.*, 
            rgt.*
        FROM Race r
        LEFT JOIN HorseType ht ON r.HorseTypeId = ht.Id
        LEFT JOIN RaceStartType rst ON r.RaceStartTypeId = rst.Id
        LEFT JOIN RaceParticipant rp ON r.Id = rp.RaceId
        LEFT JOIN Driver d ON rp.DriverSourceId = d.SourceId
        LEFT JOIN Horse h ON rp.HorseSourceId = h.SourceId
        LEFT JOIN Driver t ON rp.TrainerSourceId = t.SourceId
        LEFT JOIN RaceCartType ct ON rp.CartTypeId = ct.Id
        LEFT JOIN RaceResults rr ON rp.Id = rr.RaceParticipantId
        LEFT JOIN Race_RaceGamblingType r_rgt ON r.Id = r_rgt.RaceId
        LEFT JOIN RaceGamblingType rgt ON r_rgt.RaceGamblingTypeId = rgt.Id";

    protected override string SqlSelectEntityById => @"SELECT * FROM Race WHERE Id = @Id";
    private string SqlSelectEntityByCompetitionId => @"SELECT * FROM Race WHERE CompetitionId = @Id";

    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE r.Id = @Id";
    private string SqlSelectComplexByCompetitionId => $"{SqlSelectComplexBase} WHERE r.CompetitionId = @Id";

    /// <summary>
    ///     Retrieves a single flat race entity associated with the given competition ID.
    /// </summary>
    public Task<RaceEntity?> GetEntityByCompetitionIdAsync(string competitionId)
    {
        return QueryEntityAsync(SqlSelectEntityByCompetitionId, new { Id = competitionId });
    }

    /// <summary>
    ///     Retrieves all races associated with a specific competition, fully hydated with
    ///     participants, drivers, horses, results, and gambling types.
    /// </summary>
    public async Task<List<RaceComplex>> GetComplexByCompetitionIdAsync(string competitionId)
    {
        var param = new { Id = competitionId };
        var results = await QueryComplexListInternalAsync(SqlSelectComplexByCompetitionId, param);
        return results.ToList();
    }

    /// <summary>
    ///     Overrides the base method to fetch a single complex race object via the internal list mapper.
    /// </summary>
    protected override async Task<RaceComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    /// <summary>
    ///     Executes the multi-table query and maps the flat row matrix into a structured domain model.
    /// </summary>
    private async Task<IEnumerable<RaceComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        // Defines the mapping order corresponding to the SELECT statement
        var types = new[]
        {
            typeof(RaceEntity),
            typeof(HorseTypeComplex),
            typeof(RaceStartTypeComplex),
            typeof(RaceParticipantComplex),
            typeof(DriverComplex),
            typeof(HorseComplex),
            typeof(DriverComplex),
            typeof(RaceCartTypeComplex),
            typeof(RaceResultsComplex),
            typeof(RaceGamblingTypeComplex)
        };

        // Reads rows into a flat intermediate buffer to decouple Dapper from complex collection binding
        var flatRows = await connection.QueryAsync<FlatRaceRow>(
            sql,
            types,
            objects => new FlatRaceRow
            {
                Race = (RaceEntity)objects[0],
                HorseType = objects[1] as HorseTypeComplex,
                StartType = objects[2] as RaceStartTypeComplex,
                Participant = objects[3] as RaceParticipantComplex,
                ParticipantDriver = objects[4] as DriverComplex,
                ParticipantHorse = objects[5] as HorseComplex,
                ParticipantTrainer = objects[6] as DriverComplex,
                ParticipantCartType = objects[7] as RaceCartTypeComplex,
                ParticipantResult = objects[8] as RaceResultsComplex,
                GamblingType = objects[9] as RaceGamblingTypeComplex
            },
            param,
            splitOn: "Id");

        // Groups the flat database rows by Race ID and reconstructs the nested structural hierarchy
        return flatRows.GroupBy(row => row.Race.Id).Select(g =>
        {
            var first = g.First();

            // Aggregates distinct participants belonging to this specific race
            var participants = g
                .Where(r => r.Participant != null)
                .DistinctBy(r => r.Participant!.Id)
                .Select(r => new RaceParticipantComplex
                {
                    Id = r.Participant!.Id,
                    TrainerSourceId = r.Participant.TrainerSourceId,
                    StartNumber = r.Participant.StartNumber,
                    TrackNumber = r.Participant.TrackNumber,
                    TrackDistance = r.Participant.TrackDistance,
                    ForeShoe = r.Participant.ForeShoe,
                    HindShoe = r.Participant.HindShoe,
                    Driver = r.ParticipantDriver!,
                    Horse = r.ParticipantHorse!,
                    Trainer = r.ParticipantTrainer!,
                    CartType = r.ParticipantCartType!,
                    Result = r.ParticipantResult!
                })
                .ToList();

            // Aggregates distinct gambling types allowed for this specific race
            var gamblingTypes = g
                .Where(r => r.GamblingType != null)
                .DistinctBy(r => r.GamblingType!.Id)
                .Select(r => r.GamblingType!)
                .ToList();

            return new RaceComplex
            {
                Id = first.Race.Id,
                RaceNumber = first.Race.RaceNumber,
                StartTime = first.Race.StartTime,
                MainDistance = first.Race.MainDistance,
                Monte = first.Race.Monte,
                HorseType = first.HorseType!,
                StartType = first.StartType!,
                Participants = participants,
                GamblingTypes = gamblingTypes
            };
        }).ToList();
    }

    /// <summary>
    ///     Temporary data container used to catch unmapped multi-join rows from Dapper.
    /// </summary>
    private sealed class FlatRaceRow
    {
        public RaceEntity Race { get; init; } = null!;
        public HorseTypeComplex? HorseType { get; init; }
        public RaceStartTypeComplex? StartType { get; init; }
        public RaceParticipantComplex? Participant { get; init; }
        public DriverComplex? ParticipantDriver { get; init; }
        public HorseComplex? ParticipantHorse { get; init; }
        public DriverComplex? ParticipantTrainer { get; init; }
        public RaceCartTypeComplex? ParticipantCartType { get; init; }
        public RaceResultsComplex? ParticipantResult { get; init; }
        public RaceGamblingTypeComplex? GamblingType { get; init; }
    }
}
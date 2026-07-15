using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling races.
///     Handles retrieval of flat race entities and deep, complex object trees
///     spanning across 10 related tables.
/// </summary>
public sealed class RaceDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceEntity, RaceComplex>(configuration), IRaceDbService
{
    // Retrieves all flat races associated with a specific competition
    private const string SqlSelectEntitiesByCompetitionId =
        "SELECT * FROM Race WHERE CompetitionId = @CompetitionId";

    /// <summary>
    ///     Base query for complex race objects.
    ///     r_rgt (Race_RaceGamblingType) is omitted from the SELECT list to keep
    ///     Dapper's multi-mapping clean, but is used in the JOIN to link 'rgt'.
    /// </summary>
    private const string SqlSelectComplexBase = @"
        SELECT 
            r.*, 
            c.*, 
            ht.*, 
            rst.*, 
            rp.*, 
            d.*, 
            h.*, 
            ct.*, 
            rr.*, 
            rgt.*
        FROM Race r
        LEFT JOIN Competition c ON r.CompetitionId = c.Id
        LEFT JOIN HorseType ht ON r.HorseTypeId = ht.Id
        LEFT JOIN RaceStartType rst ON r.RaceStartTypeId = rst.Id
        LEFT JOIN RaceParticipant rp ON r.Id = rp.RaceId
        LEFT JOIN Driver d ON rp.DriverSourceId = d.SourceId
        LEFT JOIN Horse h ON rp.HorseSourceId = h.SourceId
        LEFT JOIN RaceCartType ct ON rp.CartTypeId = ct.Id
        LEFT JOIN RaceResults rr ON rp.Id = rr.RaceParticipantId
        LEFT JOIN Race_RaceGamblingType r_rgt ON r.Id = r_rgt.RaceId
        LEFT JOIN RaceGamblingType rgt ON r_rgt.RaceGamblingTypeId = rgt.Id";

    // Retrieves a flat race entity by ID
    protected override string SqlSelectEntityById =>
        "SELECT * FROM Race WHERE Id = @Id";

    // Reuses the base query to retrieve a specific complex race by ID
    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE r.Id = @Id";

    /// <summary>
    ///     Retrieves a list of flat race entities belonging to a competition.
    /// </summary>
    public Task<IEnumerable<RaceEntity>> GetEntitiesByCompetitionIdAsync(string competitionId)
    {
        return QueryEntityListAsync(SqlSelectEntitiesByCompetitionId, new { CompetitionId = competitionId });
    }

    /// <summary>
    ///     Retrieves deep, complex object trees for all races in a specific competition.
    /// </summary>
    public Task<IEnumerable<RaceComplex>> GetComplexesByCompetitionIdAsync(string competitionId)
    {
        return QueryComplexListInternalAsync($"{SqlSelectComplexBase} WHERE r.CompetitionId = @CompetitionId",
            new { CompetitionId = competitionId });
    }

    /// <summary>
    ///     Helper method for ReadSingleDbService to retrieve and return the first match of a complex model.
    /// </summary>
    protected override async Task<RaceComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    /// <summary>
    ///     Helper method to retrieve a list of simple, flat race entities.
    /// </summary>
    private async Task<IEnumerable<RaceEntity>> QueryEntityListAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();
        return await connection.QueryAsync<RaceEntity>(sql, param);
    }

    /// <summary>
    ///     Executes the SQL query and performs the heavy lifting of mapping 10 tables
    ///     into a structured, hierarchical C# object tree using Dapper and LINQ.
    /// </summary>
    private async Task<IEnumerable<RaceComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        // 10 types matching the chronological order of the columns in SqlSelectComplexBase
        var types = new[]
        {
            typeof(RaceEntity),
            typeof(CompetitionComplex),
            typeof(HorseTypeComplex),
            typeof(RaceStartTypeComplex),
            typeof(RaceParticipantComplex),
            typeof(DriverComplex),
            typeof(HorseComplex),
            typeof(RaceCartTypeComplex),
            typeof(RaceResultsComplex),
            typeof(RaceGamblingTypeComplex)
        };

        // Fetch raw data and map directly to the flat helper class FlatRaceRow.
        // splitOn tells Dapper where the transition to the next C# object begins in the column sequence.
        var flatRows = await connection.QueryAsync<FlatRaceRow>(
            sql,
            types,
            objects => new FlatRaceRow
            {
                Race = (RaceEntity)objects[0],
                Competition = objects[1] as CompetitionComplex,
                HorseType = objects[2] as HorseTypeComplex,
                StartType = objects[3] as RaceStartTypeComplex,
                Participant = objects[4] as RaceParticipantComplex,
                ParticipantDriver = objects[5] as DriverComplex,
                ParticipantHorse = objects[6] as HorseComplex,
                ParticipantCartType = objects[7] as RaceCartTypeComplex,
                ParticipantResult = objects[8] as RaceResultsComplex,
                GamblingType = objects[9] as RaceGamblingTypeComplex
            },
            param,
            splitOn: "Id,Id,Id,Id,Id,Id,Id,Id,Id"); // 9 splits for the 10 objects in the sequence

        // Group rows by Race.Id to aggregate lists of participants and gambling types per unique race
        return flatRows.GroupBy(row => row.Race.Id).Select(g =>
        {
            var first = g.First();

            // Filter and map unique participants for this race (including driver, horse, cart, and result)
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
                    CartType = r.ParticipantCartType!,
                    Result = r.ParticipantResult!
                })
                .ToList();

            // Filter and map unique gambling types (V75, V5, etc.) associated with this race
            var gamblingTypes = g
                .Where(r => r.GamblingType != null)
                .DistinctBy(r => r.GamblingType!.Id)
                .Select(r => r.GamblingType!)
                .ToList();

            // Return the fully assembled RaceComplex object
            return new RaceComplex
            {
                Id = first.Race.Id,
                RaceNumber = first.Race.RaceNumber,
                StartTime = first.Race.StartTime,
                MainDistance = first.Race.MainDistance,
                Monte = first.Race.Monte,
                Competition = first.Competition!, // Reference back to the parent competition
                HorseType = first.HorseType!,
                StartType = first.StartType!,
                Participants = participants,
                GamblingTypes = gamblingTypes
            };
        }).ToList();
    }

    /// <summary>
    ///     Private helper class representing a flat row returned from the Dapper query.
    ///     Used exclusively internally to hold mapped objects before running the LINQ grouping.
    /// </summary>
    private sealed class FlatRaceRow
    {
        public RaceEntity Race { get; init; } = null!;
        public CompetitionComplex? Competition { get; init; }
        public HorseTypeComplex? HorseType { get; init; }
        public RaceStartTypeComplex? StartType { get; init; }
        public RaceParticipantComplex? Participant { get; init; }
        public DriverComplex? ParticipantDriver { get; init; }
        public HorseComplex? ParticipantHorse { get; init; }
        public RaceCartTypeComplex? ParticipantCartType { get; init; }
        public RaceResultsComplex? ParticipantResult { get; init; }
        public RaceGamblingTypeComplex? GamblingType { get; init; }
    }
}
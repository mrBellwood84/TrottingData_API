using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling competitions.
///     Inherits base read functionality and implements custom mapping
///     of deep, complex object trees spanning across 11 related tables.
/// </summary>
public sealed class CompetitionDbService(IConfiguration configuration)
    : ReadAllDbService<CompetitionEntity, CompetitionComplex>(configuration), ICompetitionDbService
{
    // Query to retrieve only a list of all available IDs
    private const string SqlSelectAllIds =
        "SELECT Id FROM Competition";

    // Query for fast retrieval of flat entities based on a list of IDs
    private const string SqlSelectEntitiesByIds =
        "SELECT * FROM Competition WHERE Id IN @Ids";

    /// <summary>
    ///     Base query that gathers all related data to build the complex object tree.
    ///     The link table 'Race_RaceGamblingType' is included in the JOIN to connect the tables,
    ///     but omitted from the SELECT list to keep Dapper's type mapping clean and uncomplicated.
    /// </summary>
    private const string SqlSelectComplexBase = @"
        SELECT 
            c.*, 
            rc.*,
            r.*, 
            ht.*, 
            rst.*, 
            rp.*, 
            d.*, 
            h.*, 
            ct.*, 
            rr.*, 
            rgt.*
        FROM Competition c
        LEFT JOIN RaceCourse rc ON c.RaceCourseId = rc.Id
        LEFT JOIN Race r ON c.Id = r.CompetitionId
        LEFT JOIN HorseType ht ON r.HorseTypeId = ht.Id
        LEFT JOIN RaceStartType rst ON r.RaceStartTypeId = rst.Id
        LEFT JOIN RaceParticipant rp ON r.Id = rp.RaceId
        LEFT JOIN Driver d ON rp.DriverSourceId = d.SourceId
        LEFT JOIN Horse h ON rp.HorseSourceId = h.SourceId
        LEFT JOIN RaceCartType ct ON rp.CartTypeId = ct.Id
        LEFT JOIN RaceResults rr ON rp.Id = rr.RaceParticipantId
        LEFT JOIN Race_RaceGamblingType r_rgt ON r.Id = r_rgt.RaceId
        LEFT JOIN RaceGamblingType rgt ON r_rgt.RaceGamblingTypeId = rgt.Id";

    // Simple query to retrieve a flat entity by ID
    protected override string SqlSelectEntityById =>
        "SELECT * FROM Competition WHERE Id = @Id";

    // Reuses the base query to retrieve a specific complex competition by ID
    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE c.Id = @Id";

    /// <summary>
    ///     Retrieves a list of all competition IDs in the database.
    /// </summary>
    public async Task<IEnumerable<string>> GetAllIdsAsync()
    {
        await using var connection = await CreateConnection();
        return await connection.QueryAsync<string>(SqlSelectAllIds);
    }

    /// <summary>
    ///     Retrieves flat entities based on a list of specific IDs.
    /// </summary>
    public async Task<IEnumerable<CompetitionEntity>> GetEntitiesByIdsAsync(IEnumerable<string> ids)
    {
        await using var connection = await CreateConnection();
        return await connection.QueryAsync<CompetitionEntity>(SqlSelectEntitiesByIds, new { Ids = ids });
    }

    /// <summary>
    ///     Retrieves complete, deep object trees based on a list of specific IDs.
    /// </summary>
    public async Task<IEnumerable<CompetitionComplex>> GetComplexesByIdsAsync(IEnumerable<string> ids)
    {
        var query = $"{SqlSelectComplexBase} WHERE c.Id IN @Ids";
        return await QueryComplexListInternalAsync(query, new { Ids = ids });
    }

    /// <summary>
    ///     Helper method for ReadAllDbService to retrieve a single complex model.
    /// </summary>
    protected override async Task<CompetitionComplex?> QueryComplexAsync(string query, object param)
    {
        var results = await QueryComplexListInternalAsync(query, param);
        return results.FirstOrDefault();
    }

    /// <summary>
    ///     Executes the heavy query, maps the flat rows over to temporary FlatCompetitionRow
    ///     objects, and groups them together into correct, hierarchical object trees.
    /// </summary>
    private async Task<IEnumerable<CompetitionComplex>> QueryComplexListInternalAsync(string sql, object param)
    {
        await using var connection = await CreateConnection();

        // The order of types must match the chronological order of columns returned in the SELECT statement
        var types = new[]
        {
            typeof(CompetitionEntity),
            typeof(RaceCourseComplex),
            typeof(RaceEntity),
            typeof(HorseTypeComplex),
            typeof(RaceStartTypeComplex),
            typeof(RaceParticipantComplex),
            typeof(DriverComplex),
            typeof(HorseComplex),
            typeof(RaceCartTypeComplex),
            typeof(RaceResultsComplex),
            typeof(RaceGamblingTypeComplex)
        };

        // Step 1: Fetch flat rows from the database and map them to FlatCompetitionRow
        var flatRows = await connection.QueryAsync<FlatCompetitionRow>(
            sql,
            types,
            objects => new FlatCompetitionRow
            {
                Competition = (CompetitionEntity)objects[0],
                Course = objects[1] as RaceCourseComplex,
                Race = objects[2] as RaceEntity,
                HorseType = objects[3] as HorseTypeComplex,
                StartType = objects[4] as RaceStartTypeComplex,
                Participant = objects[5] as RaceParticipantComplex,
                ParticipantDriver = objects[6] as DriverComplex,
                ParticipantHorse = objects[7] as HorseComplex,
                ParticipantCartType = objects[8] as RaceCartTypeComplex,
                ParticipantResult = objects[9] as RaceResultsComplex,
                GamblingType = objects[10] as RaceGamblingTypeComplex
            },
            param,
            splitOn: "Id"); // Dapper automatically splits on every occurrence of "Id" across the columns

        // Step 2: Group flat rows per Competition to construct the hierarchy
        return flatRows.GroupBy(row => row.Competition.Id).Select(g =>
        {
            var first = g.First();

            /*
             * CIRCULAR REFERENCE HANDLING:
             * CompetitionComplex refers to a list of RaceComplex, and each RaceComplex
             * refers back to its CompetitionComplex. Since the models use "init" properties,
             * we instantiate a "shallow parent" (a basic copy without the races) to pass to the children.
             */
            var shallowParent = new CompetitionComplex
            {
                Id = first.Competition.Id,
                Date = first.Competition.Date,
                FromDirectSource = first.Competition.FromDirectSource,
                Course = first.Course!,
                Races = null! // Left unassigned here to avoid infinite loops during initialization
            };

            // Step 3: Group rows per race within this competition
            var races = g
                .Where(r => r.Race != null)
                .GroupBy(r => r.Race!.Id)
                .Select(rg =>
                {
                    var firstRaceRow = rg.First();

                    // Retrieve and map unique participants for this specific race
                    var participants = rg
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

                    // Retrieve and map unique gambling types associated with this specific race
                    var gamblingTypes = rg
                        .Where(r => r.GamblingType != null)
                        .DistinctBy(r => r.GamblingType!.Id)
                        .Select(r => r.GamblingType!)
                        .ToList();

                    // Return fully constructed RaceComplex linked to the shallow parent
                    return new RaceComplex
                    {
                        Id = firstRaceRow.Race!.Id,
                        RaceNumber = firstRaceRow.Race.RaceNumber,
                        StartTime = firstRaceRow.Race.StartTime,
                        MainDistance = firstRaceRow.Race.MainDistance,
                        Monte = firstRaceRow.Race.Monte,
                        Competition = shallowParent, // Securely passing the circular reference here
                        HorseType = firstRaceRow.HorseType!,
                        StartType = firstRaceRow.StartType!,
                        Participants = participants,
                        GamblingTypes = gamblingTypes
                    };
                })
                .ToList();

            // Step 4: Return the final, complete CompetitionComplex model populated with its races
            return new CompetitionComplex
            {
                Id = first.Competition.Id,
                Date = first.Competition.Date,
                FromDirectSource = first.Competition.FromDirectSource,
                Course = first.Course!,
                Races = races
            };
        }).ToList();
    }

    /// <summary>
    ///     Private helper class representing a flat row returned from the SQL query.
    ///     Used exclusively internally to hold mapped objects before applying the LINQ grouping.
    /// </summary>
    private sealed class FlatCompetitionRow
    {
        public CompetitionEntity Competition { get; init; } = null!;
        public RaceCourseComplex? Course { get; init; }
        public RaceEntity? Race { get; init; }
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
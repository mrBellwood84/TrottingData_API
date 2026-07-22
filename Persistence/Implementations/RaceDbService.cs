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
    : ReadAllDbService<RaceEntity, RaceComplex>(configuration), IRaceDbService
{
    private const string SqlSelectComplexBase = @"
        SELECT 
            r.*, 
            ht.*, 
            rst.*, 
            rp.*, 
            d.*, 
            dl_d.*, -- Kuskelisens
            h.*, 
            hs.*,   -- Hestekjønn
            ht_h.*, -- Hestetype (på hest-nivå, aliasert som ht_h for å unngå kollisjon med løpets ht)
            t.*, 
            dl_t.*, -- Trenerlisens
            ct.*, 
            rr.*, 
            rgt.*
        FROM Race r
        LEFT JOIN HorseType ht ON r.HorseTypeId = ht.Id
        LEFT JOIN RaceStartType rst ON r.RaceStartTypeId = rst.Id
        LEFT JOIN RaceParticipant rp ON r.Id = rp.RaceId
        LEFT JOIN Driver d ON rp.DriverSourceId = d.SourceId
        LEFT JOIN DriverLicense dl_d ON d.DriverLicenseId = dl_d.Id
        LEFT JOIN Horse h ON rp.HorseSourceId = h.SourceId
        LEFT JOIN HorseSex hs ON h.HorseSexId = hs.Id
        LEFT JOIN HorseType ht_h ON h.HorseTypeId = ht_h.Id
        LEFT JOIN Driver t ON rp.TrainerSourceId = t.SourceId
        LEFT JOIN DriverLicense dl_t ON t.DriverLicenseId = dl_t.Id
        LEFT JOIN RaceCartType ct ON rp.CartTypeId = ct.Id
        LEFT JOIN RaceResult rr ON rp.Id = rr.RaceParticipantId
        LEFT JOIN Race_RaceGamblingType r_rgt ON r.Id = r_rgt.RaceId
        LEFT JOIN RaceGamblingType rgt ON r_rgt.RaceGamblingTypeId = rgt.Id";

    protected override string SqlSelectEntities => @"SELECT * FROM Race";   
    protected override string SqlSelectEntityById => @"SELECT * FROM Race WHERE Id = @Id";
    private string SqlSelectEntityByCompetitionId => @"SELECT * FROM Race WHERE CompetitionId = @Id";

    protected override string SqlSelectComplexById => $"{SqlSelectComplexBase} WHERE r.Id = @Id";
    private string SqlSelectComplexByCompetitionId => $"{SqlSelectComplexBase} WHERE r.CompetitionId = @Id";

    /// <summary>
    ///     Retrieves a single flat race entity associated with the given competition ID.
    /// </summary>
    public Task<List<RaceEntity>> GetEntityByCompetitionIdAsync(string competitionId)
    {
        return QueryEntityListAsync(SqlSelectEntityByCompetitionId, new { Id = competitionId });
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

        // Her definerer vi samtlige 14 typer i nøyaktig samme rekkefølge som de hentes i SELECT-en
        var types = new[]
        {
            typeof(RaceEntity), // 0: r.*
            typeof(HorseTypeComplex), // 1: ht.* (Hestetype på løpsnivå)
            typeof(RaceStartTypeComplex), // 2: rst.*
            typeof(RaceParticipantComplex), // 3: rp.*
            typeof(DriverComplex), // 4: d.* (Kusk)
            typeof(DriverLicenseComplex), // 5: dl_d.* (Kuskens lisens)
            typeof(HorseComplex), // 6: h.* (Hest)
            typeof(HorseSexComplex), // 7: hs.* (Hestens kjønn)
            typeof(HorseTypeComplex), // 8: ht_h.* (Hestetype på hestenivå)
            typeof(DriverComplex), // 9: t.* (Trener)
            typeof(DriverLicenseComplex), // 10: dl_t.* (Trenerens lisens)
            typeof(RaceCartTypeComplex), // 11: ct.*
            typeof(RaceResultComplex), // 12: rr.*
            typeof(RaceGamblingTypeComplex) // 13: rgt.*
        };

        // Mapper rå-radene over i det flate mellomlagsobjektet vårt
        var flatRows = await connection.QueryAsync<FlatRaceRow>(
            sql,
            types,
            objects =>
            {
                var row = new FlatRaceRow
                {
                    Race = (RaceEntity)objects[0],
                    HorseType = objects[1] as HorseTypeComplex,
                    StartType = objects[2] as RaceStartTypeComplex,
                    Participant = objects[3] as RaceParticipantComplex,
                    ParticipantDriver = objects[4] as DriverComplex,
                    ParticipantDriverLicense = objects[5] as DriverLicenseComplex,
                    ParticipantHorse = objects[6] as HorseComplex,
                    ParticipantHorseSex = objects[7] as HorseSexComplex,
                    ParticipantHorseType = objects[8] as HorseTypeComplex,
                    ParticipantTrainer = objects[9] as DriverComplex,
                    ParticipantTrainerLicense = objects[10] as DriverLicenseComplex,
                    ParticipantCartType = objects[11] as RaceCartTypeComplex,
                    ParticipantResult = objects[12] as RaceResultComplex,
                    GamblingType = objects[13] as RaceGamblingTypeComplex
                };

                // Hydrer kusk-relasjonen
                if (row.ParticipantDriver != null) row.ParticipantDriver.License = row.ParticipantDriverLicense;

                // Hydrer heste-relasjonene
                if (row.ParticipantHorse != null)
                {
                    row.ParticipantHorse.Sex = row.ParticipantHorseSex;
                    row.ParticipantHorse.Type = row.ParticipantHorseType;
                }

                // Hydrer trener-relasjonen
                if (row.ParticipantTrainer != null) row.ParticipantTrainer.License = row.ParticipantTrainerLicense;

                return row;
            },
            param,
            splitOn: "Id");

        // Grupperer de flate radene på Race ID og bygger opp det ferdige, dype objekt-hierarkiet
        return flatRows.GroupBy(row => row.Race.Id).Select(g =>
        {
            var first = g.First();

            // Samler unike deltakere per løp (DistinctBy hindrer duplikater på grunn av gambling-typer)
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

            // Samler unike gamblingtyper for løpet
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
        public DriverLicenseComplex? ParticipantDriverLicense { get; init; }
        public HorseComplex? ParticipantHorse { get; init; }
        public HorseSexComplex? ParticipantHorseSex { get; init; }
        public HorseTypeComplex? ParticipantHorseType { get; init; }
        public DriverComplex? ParticipantTrainer { get; init; }
        public DriverLicenseComplex? ParticipantTrainerLicense { get; init; }
        public RaceCartTypeComplex? ParticipantCartType { get; init; }
        public RaceResultComplex? ParticipantResult { get; init; }
        public RaceGamblingTypeComplex? GamblingType { get; init; }
    }
}
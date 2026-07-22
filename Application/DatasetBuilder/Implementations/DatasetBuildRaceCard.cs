using System.Collections.Concurrent;
using Application.Configurations;
using Application.DatasetBuilder.Exceptions;
using Application.DatasetBuilder.Services;
using Application.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Models.Complex;
using Models.Datasets;
using Models.Entity;

namespace Application.DatasetBuilder.Implementations;

public class DatasetBuildRaceCard(
    IOptions<DatasetBuilderRules> rules,
    IReadAllRepository<CompetitionEntity, CompetitionComplex> competitionRepository,
    IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository,
    IReadSourceRepository<DriverEntity, DriverComplex> driverRepository,
    IReadSourceRepository<HorseEntity, HorseComplex> horseRepository,
    IRaceRepository raceRepository,
    IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> raceStartTypeRepository,
    IRaceParticipantRepository raceParticipantRepository,
    IRaceResultRepository raceResultRepository
) : DatasetBuilderService<DatasetRaceCard>(
    rules,
    competitionRepository,
    raceCourseRepository,
    driverRepository,
    horseRepository,
    raceRepository,
    raceParticipantRepository,
    raceResultRepository)
{
    private readonly IReadAllRepository<CompetitionEntity, CompetitionComplex> _competitionRepository = competitionRepository;
    private readonly IRaceRepository _raceRepository = raceRepository;
    private readonly IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> _raceStartTypeRepository = raceStartTypeRepository;
    private readonly IRaceParticipantRepository _raceParticipantRepository = raceParticipantRepository;

    public override async Task<List<DatasetRaceCard>> BuildAsync(string raceId)
    {
        await InitializeRaceAsync(raceId);
        var approved = CheckRules();
        if (!approved) return [];

        var result = new ConcurrentBag<DatasetRaceCard>();
        var basic = await BuildBasicDataAsync(raceId);
        

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        await Parallel.ForEachAsync(Participants, parallelOptions, async (item, _) =>
        {
            await ResolveRaceCard(basic, item, result);
        });

        return result.ToList();
    }

    private async Task ResolveRaceCard(DatasetBasic baseData, RaceParticipantComplex participant,
        ConcurrentBag<DatasetRaceCard> results)
    {
        var raceResult = await GetRaceResultByParticipant(participant.Id);
        var raceCard = new DatasetRaceCard(baseData, participant, raceResult);

        var statistics = await ResolveHorseStatsAsync(participant.Horse.SourceId, baseData.Date);
        statistics.PopulateRaceCard(raceCard);

        results.Add(raceCard);
    }

    private async Task<RaceCardStatistics> ResolveHorseStatsAsync(string horseSourceId, DateTime raceDate)
    {
        var participantData = await _raceParticipantRepository.GetComplexesByHorseAsync(horseSourceId);

        if (participantData == null)
        {
            var errorMsg = $"Participant data was expected for horse with source id: {horseSourceId}";
            throw new DatasetNoParticipantFoundException(errorMsg);
        }

        var raceCardStatistic = new RaceCardStatistics();

        var career = raceCardStatistic.Career;
        var season = raceCardStatistic.Season;
        var prevSeason = raceCardStatistic.PrevSeason;

        foreach (var item in participantData)
        {
            var participantEntity = await _raceParticipantRepository.GetEntityByIdAsync(item.Id);
            var raceEntity = await _raceRepository.GetEntityByIdAsync(participantEntity!.RaceId);
            var competitionEntity = await _competitionRepository.GetEntityByIdAsync(raceEntity!.CompetitionId);

            if (competitionEntity!.Date >= raceDate) continue;

            var year = competitionEntity.Date.Year;
            var startType = await _raceStartTypeRepository.GetComplexByIdAsync(raceEntity.RaceStartTypeId);
            var kmTime = DatasetHelpers.ParseKmTime(item.Result.KmTime);
            var place = item.Result.Place;

            if (kmTime.HasValue)
            {
                if (startType!.Type == "VOLTE")
                {
                    if (career.VolteBest == null || kmTime < career.VolteBest)
                        career.VolteBest = kmTime;

                    if (year == raceDate.Year)
                        if (season.VolteBest == null || kmTime < season.VolteBest)
                            season.VolteBest = kmTime;

                    if (year + 1 == raceDate.Year)
                        if (prevSeason.VolteBest == null || kmTime < prevSeason.VolteBest)
                            prevSeason.VolteBest = kmTime;
                }

                if (startType!.Type == "AUTO")
                {
                    if (career.AutoBest == null || kmTime < career.AutoBest)
                        career.AutoBest = kmTime;

                    if (year == raceDate.Year)
                        if (season.AutoBest == null || kmTime < season.AutoBest)
                            season.AutoBest = kmTime;

                    if (year + 1 == raceDate.Year)
                        if (prevSeason.AutoBest == null || kmTime < prevSeason.AutoBest)
                            prevSeason.AutoBest = kmTime;
                }
            }

            career.TotalRaces++;
            career.Winnings += item.Result.Price;
            if (place == 1) career.FirstPlace++;
            if (place == 2) career.SecondPlace++;
            if (place == 3) career.ThirdPlace++;

            if (year == raceDate.Year)
            {
                season.TotalRaces++;
                season.Winnings += item.Result.Price;
                if (place == 1) season.FirstPlace++;
                if (place == 2) season.SecondPlace++;
                if (place == 3) season.ThirdPlace++;
            }

            if (year + 1 == raceDate.Year)
            {
                prevSeason.TotalRaces++;
                prevSeason.Winnings += item.Result.Price;
                if (place == 1) prevSeason.FirstPlace++;
                if (place == 2) prevSeason.SecondPlace++;
                if (place == 3) prevSeason.ThirdPlace++;
            }
        }

        raceCardStatistic.Career = career;
        raceCardStatistic.Season = season;
        raceCardStatistic.PrevSeason = prevSeason;

        return raceCardStatistic;
    }
}
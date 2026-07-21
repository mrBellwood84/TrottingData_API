using System.Collections.Concurrent;
using Application.Configurations;
using Application.DatasetBuilder.Services;
using Application.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Models.Complex;
using Models.Datasets;
using Models.Entity;

namespace Application.DatasetBuilder.Implementations;

public class DatasetBuildRaceCard(
    IOptions<DatasetBuilderRules> options,
    IReadAllRepository<CompetitionEntity, CompetitionComplex> competitionRepository,
    IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository,
    IReadSourceRepository<DriverEntity, DriverComplex> driverRepository,
    IReadSourceRepository<HorseEntity, HorseComplex> horseRepository,
    IRaceRepository raceRepository,
    IRaceParticipantRepository raceParticipantRepository,
    IRaceResultRepository raceResultRepository) : DatasetBuilderService<DatasetRaceCard>(options, competitionRepository,
    raceCourseRepository, driverRepository, horseRepository, raceRepository, raceParticipantRepository,
    raceResultRepository)
{
    public override async Task<List<DatasetRaceCard>> BuildAsync(string raceId)
    {
        await InitializeRaceAsync(raceId);
        var approved = CheckRules();
        if (!approved) return [];

        var result = new ConcurrentBag<DatasetRaceCard>();
        var tasks = new List<Task>();
        // todo : create task list to await all!

        var basic = await BuildBasicDataAsync(raceId);

        foreach (var item in Participants)
        {
            var task = ResolveRaceCard(basic, item, result);
            tasks.Add(task);
        }
        
        Task.WaitAll(tasks);
        return result.ToList();
    }

    private async Task ResolveRaceCard(DatasetBasic baseData, RaceParticipantComplex participant, ConcurrentBag<DatasetRaceCard> results)
    {
        var raceResult = await GetRaceResultByParticipant(participant.Id);
        var raceCard = new DatasetRaceCard(baseData, participant, raceResult);
        results.Add(raceCard);
    }
}
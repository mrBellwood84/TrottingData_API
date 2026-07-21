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

        // todo : create concurrent bag to resolved race cards
        var result = new List<DatasetRaceCard>();
        // todo : create task list to await all!

        var basic = await BuildBasicDataAsync(raceId);

        foreach (var item in Participants)
        {
            var raceCard = new DatasetRaceCard(basic);

            result.Add(raceCard);
        }

        return result;
    }

    private Task ResolveRaceCard(DatasetRaceCard raceCard, RaceParticipantComplex participant)
    {
        return Task.CompletedTask;
    }
}
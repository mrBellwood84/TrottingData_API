using Application.Configurations;
using Application.DatasetBuilder.Services;
using Application.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Models.Complex;
using Models.Datasets;
using Models.Entity;

namespace Application.DatasetBuilder.Implementations;

public class DatasetRaceCard(
    IOptions<DatasetBuilderOptions> options,
    IReadAllRepository<CompetitionEntity, 
        CompetitionComplex> competitionRepository, 
    IReadAllRepository<HorseTypeEntity, HorseTypeComplex> horseTypeRepository, 
    IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository, 
    IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> raceStartTypeRepository, 
    IRaceRepository raceRepository) 
    : DatasetBuilderService<DatasetBasic>(options, competitionRepository, horseTypeRepository, raceCourseRepository, raceStartTypeRepository, raceRepository)
{
    public override async Task<DatasetBasic> BuildAsync(string raceId)
    {
        var result = await BuildBasicData(raceId);
        return result;
    }
}
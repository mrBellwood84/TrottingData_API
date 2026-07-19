using Application.DatasetBuilder.Services;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Datasets;
using Models.Entity;

namespace Application.DatasetBuilder.Implementations;

public class DatasetRaceCard(
    IReadAllRepository<CompetitionEntity, 
        CompetitionComplex> competitionRepository, 
    IReadAllRepository<HorseTypeEntity, HorseTypeComplex> horseTypeRepository, 
    IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository, 
    IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> raceStartTypeRepository, 
    IRaceRepository raceRepository) 
    : DatasetBuilderService<DatasetBasic>(competitionRepository, horseTypeRepository, raceCourseRepository, raceStartTypeRepository, raceRepository)
{
    public override async Task<DatasetBasic> BuildAsync(string raceId)
    {
        var result = await BuildBasicData(raceId);
        return result;
    }
}
using Application.DatasetBuilder.Exceptions;
using Application.DatasetBuilder.Interfaces;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Datasets;
using Models.Entity;

namespace Application.DatasetBuilder.Services;

public class DatasetBuilderService<T>(
    IReadAllRepository<CompetitionEntity, CompetitionComplex> competitionRepository,
    IReadAllRepository<HorseTypeEntity, HorseTypeComplex> horseTypeRepository,
    IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository,
    IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> raceStartTypeRepository,
    IRaceRepository raceRepository) : IDatasetBuilderService<T>
{   
    public virtual Task<T> BuildAsync(string raceId)
    {
        var message = "No functionality added to build method";
        throw new DatasetNoBuildMethodException(message);
    }
    
    protected async Task<DatasetBasic> BuildBasicData(string raceId)
    {
        var raceEntity = await raceRepository.GetEntityByIdAsync(raceId);
        var competitionEntity = await competitionRepository.GetEntityByIdAsync(raceEntity!.CompetitionId);
        var raceCourseEntity = await raceCourseRepository.GetEntityByIdAsync(competitionEntity!.RaceCourseId);
        var horseType = raceEntity.HorseTypeId != null ? await horseTypeRepository.GetEntityByIdAsync(raceEntity.HorseTypeId) : null;
        var startType = await raceStartTypeRepository.GetEntityByIdAsync(raceEntity.RaceStartTypeId);
        
        
        var data = new DatasetBasic()
        {
            Date = competitionEntity.Date,
            RaceCourse = raceCourseEntity!.Name,
            RaceNumber = raceEntity.RaceNumber,
            HorseType = horseType?.Type,
            StartType = startType!.Type,
            Monte = raceEntity.Monte,
        };
        
        return data;
    }
}
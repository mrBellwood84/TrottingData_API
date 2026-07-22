using Application.Cache.Interfaces;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Preloader.Service;

public class PreloaderService(
    IReadAllCache<CompetitionEntity> competitionCache,
    IReadAllDbService<CompetitionEntity, CompetitionComplex> competitionDbService,
    IRaceCache<RaceEntity> raceCache,
    IRaceDbService raceDbService,
    IRaceParticipantCache<RaceParticipantEntity> participantCache,
    IRaceParticipantDbService raceParticipantDbService) : IPreloaderService
{
    public bool CompetitionEntitiesLoaded { get; set; } = false;
    public bool RaceEntitiesLoaded { get; set; } = false;
    public bool ParticipantEntityPreloaded { get; set; } = false;
    
    public bool HorseComplexPreloaded { get; set; } = false;
    public bool DriverComplexPreloaded { get; set; } = false;

    public Task PreloadAllAsync()
    {
        var compTask = PreloadCompetitionEntitiesAsync();
        var raceTask = PreloadRaceEntitiesAsync();
        var partTask = PreloadParticipantEntitiesAsync();

        Task.WaitAll(compTask, raceTask, partTask);
        return Task.CompletedTask;
    }

    public async Task PreloadCompetitionEntitiesAsync()
    {
        if (CompetitionEntitiesLoaded) return;
        var data = await competitionDbService.GetEntitiesAsync();
        await competitionCache.SetAsync(data);
        CompetitionEntitiesLoaded = true;
    }

    public async Task PreloadRaceEntitiesAsync()
    {
        if (RaceEntitiesLoaded) return;
        var data = await raceDbService.GetEntitiesAsync();
        await raceCache.SetAsync(data);
        RaceEntitiesLoaded = true;
    }

    public async Task PreloadParticipantEntitiesAsync()
    {
        if (ParticipantEntityPreloaded) return;
        var data = await raceParticipantDbService.GetAllEntitiesAsync();
        await participantCache.SetAsync(data);
        ParticipantEntityPreloaded = true;
    }
    
    
    
}
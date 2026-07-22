namespace Application.Preloader.Service;

public interface IPreloaderService
{
    bool CompetitionEntitiesLoaded { get; set; }
    bool RaceEntitiesLoaded { get; set; }
    bool ParticipantEntityPreloaded { get; set; }
    bool HorseComplexPreloaded { get; set; }
    bool DriverComplexPreloaded { get; set; }
    Task PreloadAllAsync();
    Task PreloadCompetitionEntitiesAsync();
    Task PreloadRaceEntitiesAsync();
    Task PreloadParticipantEntitiesAsync();
}
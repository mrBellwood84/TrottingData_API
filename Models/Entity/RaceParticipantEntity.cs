using Models.Interfaces;

namespace Models.Entity;

public class RaceParticipantEntity : IEntity
{
    public string Id { get; init; }
    public string RaceId { get; init; }
    public string DriverSourceId { get; init; }
    public string HorseSourceId { get; init; }
    public string TrainerSourceId { get; init; }
    public string CartTypeId { get; init; }
    public short StartNumber { get; init; }
    public short TrackNumber { get; init; }
    public int TrackDistance { get; init; }
    public bool? ForeShoe { get; init; }
    public bool? HindShoe { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
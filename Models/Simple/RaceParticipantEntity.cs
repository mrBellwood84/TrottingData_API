namespace Models.Simple;

public class RaceParticipantEntity
{
    public string Id { get; set; } = string.Empty;
    public string RaceId { get; set; } = string.Empty;
    public string DriverSourceId { get; set; } = string.Empty;
    public string HorseSourceId { get; set; } = string.Empty;
    public string? TrainerSourceId { get; set; }
    public string? CartTypeId { get; set; }
    public short StartNumber { get; set; }
    public short TrackNumber { get; set; }
    public int TrackDistance { get; set; }
    public bool? ForeShoe { get; set; }
    public bool? HindShoe { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
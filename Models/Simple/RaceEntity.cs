namespace Models.Simple;

public class RaceEntity
{
    public string Id { get; set; } = string.Empty;
    public string CompetitionId { get; set; } = string.Empty;
    public string? HorseTypeId { get; set; }
    public string? RaceStartTypeId { get; set; }
    public short RaceNumber { get; set; }
    public DateTime? StartTime { get; set; }
    public int? MainDistance { get; set; }
    public bool? Monte { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
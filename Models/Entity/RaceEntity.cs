using Models.Interfaces;

namespace Models.Entity;

public class RaceEntity : IDbItem
{
    public string Id { get; init; }
    public string CompetitionId { get; init; }
    public string HorseTypeId { get; init; }
    public string RaceStartTypeId { get; init; }
    public short RaceNumber { get; init; }
    public DateTime StartTime { get; init; }
    public int MainDistance { get; init; }
    public bool Monte { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
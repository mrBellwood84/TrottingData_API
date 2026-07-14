using Models.Interfaces;

namespace Models.Entity;

public class CompetitionEntity : IDbItem
{
    public string Id { get; init; }
    public string RaceCourseId { get; init; }
    public DateTime Date { get; init; }
    public bool FromDirectSource { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
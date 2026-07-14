using Models.Interfaces;

namespace Models.Simple;

public class RaceStartTypeEntity : IDbItem
{
    public string Id { get; init; }
    public string Type { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
using Models.Interfaces;

namespace Models.Entity;

public class RaceStartTypeEntity : IEntity
{
    public string Id { get; init; }
    public string Type { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
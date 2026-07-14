using Models.Interfaces;

namespace Models.Entity;

public class HorseSexEntity : IEntity
{
    public string Id { get; init; }
    public string Sex { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
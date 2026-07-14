using Models.Interfaces;

namespace Models.Entity;

public class RaceCourseEntity : IEntity
{
    public string Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
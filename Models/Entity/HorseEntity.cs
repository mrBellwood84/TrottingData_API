using Models.Interfaces;

namespace Models.Entity;

public class HorseEntity : ISourcedEntity
{
    public string Id { get; init; }
    public string SourceId { get; init; }
    public string HorseSexId { get; init; }
    public string HorseTypeId { get; init; }
    public string Name { get; init; }
    public int YearOfBirth { get; init; }
    public string FatherSourceId { get; init; }
    public string MotherSourceId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
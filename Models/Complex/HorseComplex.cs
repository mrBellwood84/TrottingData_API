using Models.Interfaces;

namespace Models.Complex;

public class HorseComplex : IEntity
{
    public string Id { get; init; }
    public string SourceId { get; init; }
    public string Name { get; init; }
    public int YearOfBirth { get; init; }
    public string FatherSourceId { get; init; }
    public string MotherSourceId { get; init; }

    // Nested objects
    public HorseSexComplex Sex { get; init; }
    public HorseTypeComplex Type { get; init; }
}
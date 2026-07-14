using Models.Interfaces;

namespace Models.Complex;

public class RaceCourseComplex : IEntity
{
    public string Id { get; init; }
    public string Name { get; init; }
}
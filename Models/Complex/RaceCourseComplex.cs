using Models.Interfaces;

namespace Models.Complex;

public class RaceCourseComplex : IDbItem
{
    public string Id { get; init; }
    public string Name { get; init; }
}
using Models.Interfaces;

namespace Models.Complex;

public class CompetitionComplex : IEntity
{
    public string Id { get; init; }
    public DateTime Date { get; init; }
    public bool FromDirectSource { get; init; }

    // Nested object
    public RaceCourseComplex Course { get; init; }
}
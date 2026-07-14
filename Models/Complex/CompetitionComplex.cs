namespace Models.Complex;

public class CompetitionComplex
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool FromDirectSource { get; set; }

    // Nested object
    public RaceCourseComplex? Course { get; set; }
}
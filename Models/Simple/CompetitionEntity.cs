namespace Models.Simple;

public class CompetitionEntity
{
    public string Id { get; set; } = string.Empty;
    public string RaceCourseId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool FromDirectSource { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
namespace Models.Simple;

public class HorseEntity
{
    public string Id { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string? HorseSexId { get; set; }
    public string? HorseTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearOfBirth { get; set; }
    public string? FatherSourceId { get; set; }
    public string? MotherSourceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
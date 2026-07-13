namespace Models.Simple;

public class DriverEntity
{
    public string Id { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string DriverLicenseId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int YearOfBirth { get; set; }
    public bool Monte { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
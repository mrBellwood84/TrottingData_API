using Models.Interfaces;

namespace Models.Entity;

public class DriverEntity : IDbItem
{
    public string Id { get; init; }
    public string SourceId { get; init; }
    public string DriverLicenseId { get; init; }
    public string Name { get; init; }
    public int YearOfBirth { get; init; }
    public bool Monte { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
using Models.Interfaces;

namespace Models.Entity;

public class DriverLicenseEntity : IEntity
{
    public string Id { get; init; }
    public string Code { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
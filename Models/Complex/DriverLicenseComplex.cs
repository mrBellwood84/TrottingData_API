using Models.Interfaces;

namespace Models.Complex;

public class DriverLicenseComplex : IDbItem
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Id { get; set; } = string.Empty;
}
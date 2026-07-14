using Models.Interfaces;

namespace Models.Complex;

public class DriverLicenseComplex : IDbItem
{
    public string Id { get; init; }
    public string Code { get; init; }
    public string Description { get; init; }
}
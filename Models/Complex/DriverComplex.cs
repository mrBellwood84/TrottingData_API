using Models.Interfaces;

namespace Models.Complex;

public class DriverComplex : IDbItem
{
    public string Id { get; init; }
    public string SourceId { get; init; }
    public string Name { get; init; }
    public int YearOfBirth { get; init; }
    public bool Monte { get; init; }

    // Nested object
    public DriverLicenseComplex License { get; init; }
}
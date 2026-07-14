using Models.Interfaces;

namespace Models.Complex;

public class RaceGamblingTypeComplex : IDbItem
{
    public string Id { get; init; }
    public string Type { get; init; }
}
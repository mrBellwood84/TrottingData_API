using Models.Interfaces;

namespace Models.Complex;

public class RaceCartTypeComplex : IDbItem
{
    public string Id { get; init; }
    public string Type { get; init; }
}
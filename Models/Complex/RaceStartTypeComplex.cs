using Models.Interfaces;

namespace Models.Complex;

public class RaceStartTypeComplex : IDbItem
{
    public string Id { get; init; }
    public string Type { get; init; }
}
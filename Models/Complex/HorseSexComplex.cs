using Models.Interfaces;

namespace Models.Complex;

public class HorseSexComplex : IDbItem
{
    public string Id { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
}
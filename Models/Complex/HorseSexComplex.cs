using Models.Interfaces;

namespace Models.Complex;

public class HorseSexComplex : IEntity
{
    public string Id { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
}
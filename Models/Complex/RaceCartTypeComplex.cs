using Models.Interfaces;

namespace Models.Complex;

public class RaceCartTypeComplex : IEntity
{
    public string Id { get; init; }
    public string Type { get; init; }
}
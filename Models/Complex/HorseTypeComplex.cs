using Models.Interfaces;

namespace Models.Complex;

public class HorseTypeComplex : IEntity
{
    public string Id { get; init; }
    public string Type { get; init; }
}
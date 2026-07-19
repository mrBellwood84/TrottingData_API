using Models.Interfaces;

namespace Models.Complex;

public class RaceResultComplex : IEntity
{
    public string Id { get; init; }
    public short Place { get; init; }
    public string KmTime { get; init; }
    public int Odds { get; init; }
    public int Price { get; init; }
    public bool Scratched { get; init; }
    public bool Disqualified { get; init; }
    public bool Broke { get; init; }
    public bool RRemark { get; init; }
    public bool GRemark { get; init; }
}
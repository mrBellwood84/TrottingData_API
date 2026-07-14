using Models.Interfaces;

namespace Models.Complex;

public class RaceParticipantComplex : IDbItem
{
    public string Id { get; init; }
    public string TrainerSourceId { get; init; }
    public short StartNumber { get; init; }
    public short TrackNumber { get; init; }
    public int TrackDistance { get; init; }
    public bool ForeShoe { get; init; }
    public bool HindShoe { get; init; }

    // Nested objects
    public DriverComplex Driver { get; init; }
    public HorseComplex Horse { get; init; }
    public RaceCartTypeComplex CartType { get; init; }
    public RaceResultsComplex Result { get; init; }
}
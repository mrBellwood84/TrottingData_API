namespace Models.Complex;

public class RaceParticipantComplex
{
    public string Id { get; set; } = string.Empty;
    public string? TrainerSourceId { get; set; }
    public short StartNumber { get; set; }
    public short TrackNumber { get; set; }
    public int TrackDistance { get; set; }
    public bool? ForeShoe { get; set; }
    public bool? HindShoe { get; set; }

    // Nested objects
    public DriverComplex? Driver { get; set; }
    public HorseComplex? Horse { get; set; }
    public RaceCartTypeComplex? CartType { get; set; }
    public RaceResultsComplex? Result { get; set; }
}
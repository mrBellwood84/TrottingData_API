namespace Models.Complex;

public class RaceComplex
{
    public string Id { get; set; } = string.Empty;
    public short RaceNumber { get; set; }
    public DateTime? StartTime { get; set; }
    public int? MainDistance { get; set; }
    public bool? Monte { get; set; }

    public CompetitionComplex? Competition { get; set; }
    public HorseTypeComplex? HorseType { get; set; }
    public RaceStartTypeComplex? StartType { get; set; }

    public List<RaceParticipantComplex> Participants { get; set; } = new();
    public List<RaceGamblingTypeComplex> GamblingTypes { get; set; } = new();
}
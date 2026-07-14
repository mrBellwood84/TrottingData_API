namespace Models.Complex;

public class RaceComplex
{
    public string Id { get; init; }
    public short RaceNumber { get; init; }
    public DateTime StartTime { get; init; }
    public int MainDistance { get; init; }
    public bool Monte { get; init; }

    public CompetitionComplex Competition { get; init; }
    public HorseTypeComplex HorseType { get; init; }
    public RaceStartTypeComplex StartType { get; init; }

    public List<RaceParticipantComplex> Participants { get; init; }
    public List<RaceGamblingTypeComplex> GamblingTypes { get; init; }
}
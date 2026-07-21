namespace Models.Datasets;

public class DatasetBasic
{
    // competition and race related data
    public DateTime Date { get; init; }
    public string RaceCourse { get; init; }
    public int RaceNumber { get; init; }
    public string HorseType { get; init; }
    public string StartType { get; init; }
    public bool Monte { get; init; }
}
namespace Models.Datasets;

public class DatasetBasic
{
    // competition and race related data
    public DateTime Date { get; set; }
    public string RaceCourse { get; set; }
    public int RaceNumber { get; set; }
    public string HorseType { get; set; }
    public string StartType { get; set; }
    public bool Monte { get; set; }
}
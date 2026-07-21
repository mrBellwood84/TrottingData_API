namespace Models.Datasets;

public class DatasetRaceCard : DatasetBasic
{
    
    public DatasetRaceCard() {}

    public DatasetRaceCard(DatasetBasic data)
    {
        Date = data.Date;
        RaceCourse = data.RaceCourse;
        RaceNumber = data.RaceNumber;
        HorseType = data.HorseType;
        StartType = data.StartType;
        Monte = data.Monte;
    }
    
    public int StartNumber { get; set; }
    public int Distance { get; set; }
    public int Track { get; set; }
    
    public string DriverName { get; set; }
    
    public string HorseName { get; set; }
    public string HorseAge { get; set; }
    public string HorseSex { get; set; }
    
    // span?
    public double HorseVolte { get; set; }
    public double HorseAuto { get; set; }
    public int HorseWinnings { get; set; }
    public bool HorseForeShoe { get; set; }
    public bool HorseHindShoe { get; set; }
    
    public int HorseRaceTotalYear { get; set; }
    public int HorseRace1PlaceYear { get; set; }
    public double HorseRace1PlaceYearPercent { get; set; }
    public int HorseRace2PlaceYear { get; set; }
    public double HorseRace2PlaceYearPercent { get; set; }
    public int HorseRace3PlaceYear { get; set; }
    public double HorseRace3PlaceYearPercent { get; set; }
    
    public int HorseRaceTotalPrevYear { get; set; }
    public int HorseRace1PlacePrevYear { get; set; }
    public double HorseRace1PlacePrevYearPercent { get; set; }
    public int HorseRace2PlacePrevYear { get; set; }
    public double HorseRace2PlacePrevYearPercent { get; set; }
    public int HorseRace3PlacePrevYear { get; set; }
    public double HorseRace3PlacePrevYearPercent { get; set; }
    
    public int HorseRaceTotalCareer { get; set; }
    public int HorseRace1PlaceCareer { get; set; }
    public  double HorseRace1PlaceCareerPercent { get; set; }
    public int HorseRace2PlaceCareer { get; set; }
    public double HorseRace2PlaceCareerPercent { get; set; }
    public int HorseRace3PlaceCareer { get; set; }
    public double HorseRace3PlaceCareerPercent { get; set; }
}
using Models.Complex;

namespace Models.Datasets;

public class DatasetRaceCard : DatasetBasic
{
    
    public DatasetRaceCard() {}

    public DatasetRaceCard(
        DatasetBasic data, 
        RaceParticipantComplex participant,
        RaceResultComplex results)
    {
        Date = data.Date;
        RaceCourse = data.RaceCourse;
        RaceNumber = data.RaceNumber;
        HorseType = data.HorseType;
        StartType = data.StartType;
        Monte = data.Monte;

        StartNumber = participant.StartNumber;
        Distance = participant.TrackDistance;
        Track = participant.TrackNumber;
        CartType = participant.CartType?.Type;
        ForeShoe = participant.ForeShoe;
        HindShoe = participant.HindShoe;

        DriverName = participant.Driver.Name;
        HorseName = participant.Horse.Name;

        // todo : add horse age:
        HorseAge = -1;
        HorseSex = participant.Horse.Sex.Sex;
        
        Place = results.Place;
        // todo : parse kmTime
        KmTime = -1;
        Odds = results.Odds;
        GRemark = results.GRemark;  
    }

    // race specific data
    public int StartNumber { get; set; }
    public int Distance { get; set; }
    public int Track { get; set; }
    public string CartType { get; set; }
    public bool? ForeShoe { get; set; }
    public bool? HindShoe { get; set; }
    
    // participant data
    public string DriverName { get; set; }
    public string HorseName { get; set; }
    public int HorseAge { get; set; }
    public string HorseSex { get; set; }
    
    // aggregated data
    public double HorseVolteMaxCareer { get; set; }
    public double HorseVolteMaxSeason { get; set; }
    public double HorseAutoMacCareer { get; set; }
    public double HorseAutoMaxSeason { get; set; }
    public int HorseWinningsSeason { get; set; }
    
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
    
    // todo : get shapes for last 5 races!!!
    
    // static result data for prediction!
    public int Place { get; set; }
    public double KmTime { get; set; }
    public int? Odds { get; set; }
    public bool GRemark { get; set; }
}
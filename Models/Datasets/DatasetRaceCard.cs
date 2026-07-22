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

        HorseAge = DatasetHelpers.CalculateHorseAge(data.Date, participant.Horse.YearOfBirth);
        HorseSex = participant.Horse.Sex.Sex;

        Place = results.Place;
        FirstPlace = results.Place == 1;
        SecondPlace = results.Place == 2;
        ThirdPlace = results.Place == 3;
        HavePlace = results.Place is >= 1 and <= 3;
        KmTime = DatasetHelpers.ParseKmTime(results.KmTime);
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
    public int? HorseAge { get; set; }
    public string HorseSex { get; set; }
    
    // historic results
    public double? VolteBestCareer { get; set; }
    public double? AutoBestCareer { get; set; }
    public int WinningsCareer { get; set; }
    public int TotalRacesCareer { get; set; }
    public int TotalFirstPlaceCareer { get; set; }
    public double PercentFirstPlaceCareer { get; set; }
    public int TotalSecondPlaceCareer { get; set; }
    public double PercentSecondPlaceCareer { get; set; }
    public int TotalThirdPlaceCareer { get; set; }
    public double PercentThirdYearCareer { get; set; }
    
    public double? VolteBestSeason { get; set; }
    public double? AutoBestSeason { get; set; }
    public int WinningsSeason { get; set; }
    public int TotalRacesSeason { get; set; }
    public int TotalFirstPlaceSeason { get; set; }
    public double PercentFirstPlaceSeason { get; set; }
    public int TotalSecondPlaceSeason { get; set; }
    public double PercentSecondPlaceSeason { get; set; }
    public int TotalThirdPlaceSeason { get; set; }
    public double PercentThirdPlaceSeason { get; set; }
    
    public double? VolteBestPrevSeason { get; set; }
    public double? AutoBestPrevSeason { get; set; }
    public int WinningsPrevSeason { get; set; }
    public int TotalRacesPrevSeason { get; set; }
    public int TotalFirstPlacePrevSeason { get; set; }
    public double PercentFirstPlacePrevSeason { get; set; }
    public int TotalSecondPlacePrevSeason { get; set; }
    public double PercentSecondPlacePrevSeason { get; set; }
    public int TotalThirdPlacePrevSeason { get; set; }
    public double PercentThirdPlacePrevSeason { get; set; }
    
    // todo : get shapes for last 5 races!!!
    
    // static result data for prediction!
    public short Place { get; set; }
    public bool FirstPlace { get; set; }
    public bool SecondPlace { get; set; }
    public bool ThirdPlace { get; set; }
    public bool HavePlace { get; set; }
    public double? KmTime { get; set; }
    public int? Odds { get; set; }
    public bool GRemark { get; set; }
    
    
}
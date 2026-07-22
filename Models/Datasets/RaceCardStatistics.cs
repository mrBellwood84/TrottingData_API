namespace Models.Datasets;

public class RaceCardStatistics
{
    public ParticipantPeriodStats Career { get; set; } = new ParticipantPeriodStats();
    public ParticipantPeriodStats Season { get; set; } = new ParticipantPeriodStats();
    public ParticipantPeriodStats PrevSeason { get; set; } = new ParticipantPeriodStats();

    public void PopulateRaceCard(DatasetRaceCard raceCard)
    {
        raceCard.VolteBestCareer = Career.VolteBest;
        raceCard.VolteBestSeason = Season.VolteBest;
        raceCard.VolteBestPrevSeason = PrevSeason.VolteBest;

        raceCard.AutoBestCareer = Career.AutoBest;
        raceCard.AutoBestSeason = Season.AutoBest;
        raceCard.AutoBestPrevSeason = PrevSeason.AutoBest;

        raceCard.WinningsCareer = Career.Winnings;
        raceCard.WinningsSeason = Season.Winnings;
        raceCard.WinningsPrevSeason = PrevSeason.Winnings;
        
        raceCard.TotalRacesCareer = Career.TotalRaces;
        raceCard.TotalFirstPlaceCareer = Career.FirstPlace;
        raceCard.TotalSecondPlaceCareer = Career.SecondPlace;
        raceCard.TotalThirdPlaceCareer = Career.ThirdPlace;
        raceCard.PercentFirstPlaceCareer = Career.PercentFirst;
        raceCard.PercentSecondPlaceCareer = Career.PercentSecond;
        raceCard.PercentThirdYearCareer = Career.PercentThird;
        
        raceCard.TotalRacesSeason = Season.TotalRaces;
        raceCard.TotalFirstPlaceSeason = Season.FirstPlace;
        raceCard.TotalSecondPlaceSeason = Season.SecondPlace;
        raceCard.TotalThirdPlaceSeason = Season.ThirdPlace;
        raceCard.PercentFirstPlaceSeason = Season.PercentFirst;
        raceCard.PercentSecondPlaceSeason = Season.PercentSecond;
        raceCard.PercentThirdPlaceSeason = Season.PercentThird;

        raceCard.TotalRacesPrevSeason = PrevSeason.TotalRaces;
        raceCard.TotalFirstPlacePrevSeason = PrevSeason.FirstPlace;
        raceCard.TotalSecondPlacePrevSeason = PrevSeason.SecondPlace;
        raceCard.TotalThirdPlacePrevSeason = PrevSeason.ThirdPlace;
        raceCard.PercentFirstPlacePrevSeason = PrevSeason.PercentFirst;
        raceCard.PercentSecondPlacePrevSeason = PrevSeason.PercentSecond;
        raceCard.PercentThirdPlacePrevSeason = PrevSeason.PercentThird;
    }
}
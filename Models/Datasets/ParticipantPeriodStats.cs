namespace Models.Datasets;

public struct ParticipantPeriodStats
{
    public ParticipantPeriodStats() {}
    
    public double? VolteBest { get; set; } = null;
    public double? AutoBest { get; set; } = null;
    public int Winnings { get; set; } = 0;

    public int TotalRaces { get; set; } = 0;
    public int FirstPlace { get; set; } = 0;
    public int SecondPlace { get; set; } = 0;
    public int ThirdPlace { get; set; } = 0;

    public double PercentFirst
    {
        get
        {
            if (TotalRaces == 0) return 0;
            return (double)FirstPlace / TotalRaces * 100;
        }
    }

    public double PercentSecond
    {
        get
        {
            if (TotalRaces == 0) return 0;
            return (double)SecondPlace / TotalRaces * 100;
        }
    }

    public double PercentThird
    {
        get
        {
            if (TotalRaces == 0) return 0;
            return (double)ThirdPlace / TotalRaces * 100;   
        }
    }
}
namespace Models.Complex;

public class RaceResultsComplex
{
    public string Id { get; set; } = string.Empty;
    public short? Place { get; set; }
    public string? KmTime { get; set; }
    public int? Odds { get; set; }
    public int? Price { get; set; }
    public bool Scratched { get; set; }
    public bool Disqualified { get; set; }
    public bool Broke { get; set; }
    public bool RRemark { get; set; }
    public bool GRemark { get; set; }
}
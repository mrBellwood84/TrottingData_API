namespace Models.Complex;

public class HorseComplex
{
    public string Id { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int YearOfBirth { get; set; }
    public string? FatherSourceId { get; set; }
    public string? MotherSourceId { get; set; }
    
    // Nested objects
    public HorseSexComplex? Sex { get; set; }
    public HorseTypeComplex? Type { get; set; }
}
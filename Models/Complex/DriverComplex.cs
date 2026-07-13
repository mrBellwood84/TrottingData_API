namespace Models.Complex;

public class DriverComplex
{
    public string Id { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int YearOfBirth { get; set; }
    public bool Monte { get; set; }
    
    // Nested object
    public DriverLicenseComplex? License { get; set; } 
}
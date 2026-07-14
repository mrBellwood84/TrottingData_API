namespace Models.Entities;

public class ModelPolicy<T>
{
    public bool AllowAllSimple { get; init; } = false;
    public bool AllowAllComplex { get; init; } = false;
}
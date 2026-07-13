namespace Models.Entities;

public class EntityPolicy<T>
{
    public bool AllowAllSimple { get; init; } = false;
    public bool AllowAllComplex { get; init; } = false;
}
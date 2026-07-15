namespace Models.Shared;

public class ModelPolicy<T>
{
    public bool AllowIdList { get; init; } = false;
    public bool AllowGetAll { get; init; } = false;
}
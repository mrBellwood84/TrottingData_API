namespace Models.Shared;

public class ModelPolicy<T>
{
    public bool AllowIdList { get; init; }
    public bool AllowGetAll { get; init; }
}
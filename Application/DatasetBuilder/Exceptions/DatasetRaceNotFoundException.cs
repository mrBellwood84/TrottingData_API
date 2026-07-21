namespace Application.DatasetBuilder.Exceptions;

public class DatasetRaceNotFoundException : Exception
{
    public DatasetRaceNotFoundException() { }
    public DatasetRaceNotFoundException(string message) : base(message) { }
    public DatasetRaceNotFoundException(string message, Exception inner) : base(message, inner) { }
}
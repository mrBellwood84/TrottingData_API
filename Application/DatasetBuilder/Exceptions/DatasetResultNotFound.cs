namespace Application.DatasetBuilder.Exceptions;

public class DatasetResultNotFound : Exception
{
    public DatasetResultNotFound() {}
    public DatasetResultNotFound(string message) : base(message) {}
    public DatasetResultNotFound(string message, Exception inner) : base(message, inner) {}
}
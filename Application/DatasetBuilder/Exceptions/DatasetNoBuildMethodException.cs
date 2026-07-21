namespace Application.DatasetBuilder.Exceptions;

public class DatasetNoBuildMethodException : Exception
{
    public DatasetNoBuildMethodException()
    {
    }

    public DatasetNoBuildMethodException(string message) : base(message)
    {
    }

    public DatasetNoBuildMethodException(string message, Exception inner) : base(message, inner)
    {
    }
}
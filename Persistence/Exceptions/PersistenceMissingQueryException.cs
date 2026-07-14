namespace Persistence.Exceptions;

public class PersistenceMissingQueryException : Exception
{
    public PersistenceMissingQueryException()
    {
    }

    public PersistenceMissingQueryException(string message) : base(message)
    {
    }

    public PersistenceMissingQueryException(string message, Exception inner) : base(message, inner)
    {
    }
}
namespace Persistence.Exceptions;

public class PersistenceQueryNotAllowedException : Exception
{
    public PersistenceQueryNotAllowedException()
    {
    }

    public PersistenceQueryNotAllowedException(string message) : base(message)
    {
    }

    public PersistenceQueryNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}
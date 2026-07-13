namespace Persistence.Exceptions;

public class PersistenceNotImplementedException : Exception
{
    public PersistenceNotImplementedException()
    {
    }
    
    public PersistenceNotImplementedException(string message) : base(message)
    {
    }

    public PersistenceNotImplementedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
namespace Persistence.Exceptions;

/// <summary>
/// Exception thrown when a database query or persistence operation is attempted 
/// but blocked by the active security or access policy.
/// </summary>
/// <remarks>
/// This exception is typically thrown when a <c>ModelPolicy</c> restricts access 
/// to specific operations (for example, if <c>policy.AllowGetAll</c> is set to <c>false</c> 
/// and a client attempts to retrieve all records).
/// </remarks>
public class PersistenceQueryNotAllowedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceQueryNotAllowedException"/> class.
    /// </summary>
    public PersistenceQueryNotAllowedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceQueryNotAllowedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PersistenceQueryNotAllowedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceQueryNotAllowedException"/> class with a specified error message 
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
    public PersistenceQueryNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}
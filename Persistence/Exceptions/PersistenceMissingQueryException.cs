namespace Persistence.Exceptions;

/// <summary>
/// Exception thrown when a required SQL query string is null or empty within a database service.
/// </summary>
/// <remarks>
/// This exception acts as a guardrail during development, signaling that a concrete service 
/// implementing <c>DbService</c> has not properly defined its required SQL queries in its constructor.
/// </remarks>
public class PersistenceMissingQueryException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceMissingQueryException"/> class.
    /// </summary>
    public PersistenceMissingQueryException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceMissingQueryException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PersistenceMissingQueryException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceMissingQueryException"/> class with a specified error message 
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
    public PersistenceMissingQueryException(string message, Exception inner) : base(message, inner)
    {
    }
}
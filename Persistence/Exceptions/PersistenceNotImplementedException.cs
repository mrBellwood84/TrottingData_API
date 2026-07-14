namespace Persistence.Exceptions;

/// <summary>
/// Exception thrown when a specific complex persistence operation has not been overridden or implemented 
/// in a concrete database service subclass.
/// </summary>
/// <remarks>
/// This exception is typically thrown by the base <c>DbService</c> class's virtual "Logic" methods 
/// (such as <c>GetAllComplexLogicAsync</c> and <c>GetComplexByIdLogicAsync</c>) to enforce that 
/// subclasses must provide their own implementation if complex data loading is requested.
/// </remarks>
public class PersistenceNotImplementedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceNotImplementedException"/> class.
    /// </summary>
    public PersistenceNotImplementedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceNotImplementedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PersistenceNotImplementedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceNotImplementedException"/> class with a specified error message 
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
    public PersistenceNotImplementedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
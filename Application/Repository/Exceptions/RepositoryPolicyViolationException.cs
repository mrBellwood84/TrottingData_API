namespace Application.Repository.Exceptions;

/// <summary>
///     Thrown when a repository operation is blocked by an active model policy.
/// </summary>
public class RepositoryPolicyViolationException : Exception
{
    public RepositoryPolicyViolationException()
        : base("The requested repository operation is disallowed by policy.")
    {
    }

    public RepositoryPolicyViolationException(string message)
        : base(message)
    {
    }

    public RepositoryPolicyViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
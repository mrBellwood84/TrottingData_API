namespace Application.DatasetBuilder.Exceptions;

public class DatasetNoParticipantFoundException : Exception
{
    public DatasetNoParticipantFoundException()
    {
    }
    public DatasetNoParticipantFoundException(string message) : base(message)
    {}

    public DatasetNoParticipantFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}
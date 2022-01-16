using System.Runtime.Serialization;

namespace MyFemsApi.Exceptions;

public class InvalidDataException : ApiException
{
    public InvalidDataException()
    {
    }

    public InvalidDataException(string? message) : base(message)
    {
    }

    public InvalidDataException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidDataException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
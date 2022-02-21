using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace RestApiClient;

public class RequestException : Exception
{
    private const string _defaultMessage = "Request handled with error.";

    internal RequestException() : base(_defaultMessage)
    {
    }

    internal RequestException(string? message) : base(message)
    {
    }

    internal RequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    private protected RequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    internal static RequestException NullResponce<T>([CallerMemberName] string memberName = "")
        => new($"{_defaultMessage} Responce is empty. Method name - {memberName}. Expected responce type - {typeof(T).Name}");
}

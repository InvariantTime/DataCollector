
namespace DataCollector.Messaging.Core;

public class MessageOptions
{
    public Type MessageType { get; }

    public Uri Endpoint { get; }

    public MessageOptions(Type type, Uri endpoint)
    {
        MessageType = type;
        Endpoint = endpoint;
    }


    public static MessageOptions CreateDefault(Type messageType)//TODO: different implementations
    {
        var fullName = messageType.FullName ?? string.Empty;
        string uriString = $"queue://{fullName.Replace('.', '/')}";

        return new MessageOptions(messageType, new Uri(uriString));
    }
}

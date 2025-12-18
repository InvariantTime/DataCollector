
namespace DataCollector.Messaging.Core;

public class MessageOptions
{
    public Type MessageType { get; }

    public MessageEndpoint Endpoint { get; }

    public MessageOptions(Type type, MessageEndpoint endpoint)
    {
        MessageType = type;
        Endpoint = endpoint;
    }


    public static MessageOptions CreateDefault(Type messageType)//TODO: different implementations
    {
        var fullName = messageType.FullName ?? string.Empty;

        return new MessageOptions(messageType, new MessageEndpoint(fullName, MessageEndpointTypes.Queue));
    }
}

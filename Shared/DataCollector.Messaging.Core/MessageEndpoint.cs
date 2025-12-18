namespace DataCollector.Messaging.Core;

public readonly record struct MessageEndpoint(string Route, MessageEndpointTypes Type); //TODO: Uri + abstract parser ?

public enum MessageEndpointTypes
{
    Queue = 0,
    Fanout = 1,
    Direct = 2,
    Topic = 3
}
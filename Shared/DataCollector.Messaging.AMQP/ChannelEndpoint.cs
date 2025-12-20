namespace DataCollector.Messaging.AMQP;

public readonly struct ChannelEndpoint
{
    public string Exchange { get; init; }

    public string Queue { get; init; }

    public string Route { get; init; }
}

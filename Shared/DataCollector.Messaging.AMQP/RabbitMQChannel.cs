using DataCollector.Messaging.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DataCollector.Messaging.AMQP;

internal class RabbitMQChannel : IBrokerModel
{
    private const string _queueScheme = "queue";

    private readonly IChannel _instance;
    private IAsyncBasicConsumer? _consumerHolder;

    public RabbitMQChannel(IChannel instance)
    {
        _instance = instance;
    }

    public async Task SendMessageAsync(byte[] data, Uri uri)
    {
        var endpoint = await ConfigureEndpointAsync(uri);

        await _instance.BasicPublishAsync(endpoint.Exchange, endpoint.Route, data);
    }

    public async Task SubscribeMessageAsync(Func<MessageCallbackData, Task> nativeCallback, Uri uri)
    {
        var endpoint = await ConfigureEndpointAsync(uri);

        var consumer = new AsyncEventingBasicConsumer(_instance);

        consumer.ReceivedAsync += (o, e) =>
        {
            var arg = new MessageCallbackData
            {
                Payload = e.Body.ToArray(),
                Route = e.RoutingKey
            };

            return nativeCallback.Invoke(arg);
        };

        await _instance.BasicConsumeAsync(endpoint.Queue, true, consumer);

        _consumerHolder = consumer;
    }

    public void Dispose()
    {
        _instance.CloseAsync().GetAwaiter().GetResult();
        _instance.Dispose();
    }

    private async Task<ChannelEndpoint> ConfigureEndpointAsync(Uri endpoint)
    {
        string scheme = endpoint.Scheme;
        string name = endpoint.Host;
        string route = endpoint.AbsolutePath.Substring(1).Replace('/', '.');
        var @params = ParseParameters(endpoint.Query);//TODO: bad code, need refactoring

        var queue = await _instance.QueueDeclareAsync(scheme == _queueScheme ? name : string.Empty, false, false, false);

        if (scheme != _queueScheme)
        {
            var type = ParseExchangeType(scheme);

            var durableRaw = @params.FirstOrDefault(x => x.Key == "durable");
            bool result = bool.TryParse(durableRaw.Value, out var durable);

            await _instance.ExchangeDeclareAsync(name, type, result == true ? durable : false);
            await _instance.QueueBindAsync(queue.QueueName, name, route);
        }

        return new ChannelEndpoint
        {
            Queue = queue.QueueName,
            Exchange = scheme != _queueScheme ? name : string.Empty,
            Route = route
        };
    }

    private static string ParseExchangeType(string scheme)
    {
        return scheme switch
        {
            "fanout" => ExchangeType.Fanout,
            "direct" => ExchangeType.Direct,
            "topic" => ExchangeType.Topic,
            _ => throw new NotSupportedException()
        };
    }

    private static IEnumerable<KeyValuePair<string, string>> ParseParameters(string query)
    {
        if (query == string.Empty)
            yield break;

        string[] parameters = query.Substring(1).Split('&');

        for (int i = 0; i < parameters.Length; i++)
        {
            string[] parts = parameters[i].Split('=');
            yield return new KeyValuePair<string, string>(parts[0], parts[1]);
        }
    }
}

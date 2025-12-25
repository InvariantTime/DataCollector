using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;

namespace DataCollector.Server.Infrastructure.Consumers;

public class ClientScanMessageConsumer : IMessageConsumer<ScanMessage>
{
    private readonly ISessionService _sessions;
    private readonly IProductService _products;

    public ClientScanMessageConsumer(ISessionService sessions, IProductService products)
    {
        _sessions = sessions;
        _products = products;
    }

    public async Task ConsumeAsync(ConsumeContext<ScanMessage> context)
    {
        var elements = context.Route.Split('.');

        if (elements.Length < 2 || Guid.TryParse(elements[1], out var id) == false)
            return;

        bool hasSession = _sessions.Sessions.TryGetValue(id, out var session);

        if (hasSession == false)
            return;

        var product = await _products.GetProductByBarCode(context.Message.Barcode);
        var uri = new Uri($"topic://amq.topic/devices/{id}/responce/message?durable=true");

        if (product.IsSuccess == false)
        {
            await context.PublishAsync(new NotifyClientMessage("Error", "There is no such product"), uri);
            return;
        }

        await context.PublishAsync(new NotifyClientMessage(product.Value!.Name, product.Value.Description), uri);
    }
}

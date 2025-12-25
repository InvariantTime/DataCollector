using DataCollector.Domain;
using DataCollector.Messaging.Core.Consuming;
using DataCollector.Server.Infrastructure.Sessions;
using DataCollector.Server.Services.DTOs;
using DataCollector.Server.Services.Interfaces;
using DataCollector.Shared.Messages;

namespace DataCollector.Server.Infrastructure.Consumers;

public class ClientAddProductMessageConsumer : IMessageConsumer<AddProductMessage>
{
    private readonly ISessionService _sessions;
    private readonly IProductService _products;

    public ClientAddProductMessageConsumer(ISessionService sessions, IProductService products)
    {
        _sessions = sessions;
        _products = products;
    }

    public async Task ConsumeAsync(ConsumeContext<AddProductMessage> context)
    {
        var elements = context.Route.Split('.');

        if (elements.Length < 2 || Guid.TryParse(elements[1], out var id) == false)
            return;

        bool hasSession = _sessions.Sessions.TryGetValue(id, out var session);

        if (hasSession == false)
            return;

        var user = session!.User;
        var uri = new Uri($"topic://amq.topic/devices/{id}/responce/message?durable=true");

        if (user.Role < UserRoles.Publisher)
        {
            await context.PublishAsync(new NotifyClientMessage("Error", "You have not permissions"), uri);
            return;
        }

        var data = context.Message;
        var result = await _products.AddProduct(new CreateProductDTO(data.Barcode, data.Name, data.Description));

        if (result.IsSuccess == false)
        {
            await context.PublishAsync(new NotifyClientMessage("Error", result.Error), uri);
            return;
        }

        await context.PublishAsync(new NotifyClientMessage("Success", $"{data.Name} added"), uri);
    }
}

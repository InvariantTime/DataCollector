using DataCollector.Messaging.Core;
using DataCollector.Messaging.Core.Rpc;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public class ScanninService : IScanningService
{
    private static readonly TimeSpan _remoteTimeout = TimeSpan.FromSeconds(5);

    private readonly IMessageBroker _broker;
    private readonly ISessionProvider _session;

    public ScanninService(IMessageBroker broker, ISessionProvider session)
    {
        _broker = broker;
        _session = session;
    }

    public async Task<NotifyMessageDTO> AddProductAsync(AddProductDTO dto)
    {
        if (_session.Session == null)
            return new NotifyMessageDTO("Error", "Unable to get session", NotifyTypes.Error);

        var session = _session.Session;
        var responceUri = session.CreateResponceUri("message");
        var requestUri = session.CreateRequestUri("add");

        var cancellation = new CancellationTokenSource(_remoteTimeout);
        using var remote = new RemoteFunction<AddProductMessage, NotifyClientMessage>(_broker, responceUri);

        var message = new AddProductMessage(dto.Barcode, dto.Name, dto.Description);
        var result = await remote.ExecuteRemoteAsync(message, requestUri, cancellation.Token);

        if (result == null)
            return new NotifyMessageDTO("Error", "Unable to get responce", NotifyTypes.Error);

        return new NotifyMessageDTO(result.Title, result.Content, NotifyTypes.Message);
    }

    public async Task<NotifyMessageDTO> ScanAsync(string barcode)
    {
        if (_session.Session == null)
            return new NotifyMessageDTO("Error", "Unable to get session", NotifyTypes.Error);

        var session = _session.Session;
        var responceUri = session.CreateResponceUri("message");
        var requestUri = session.CreateRequestUri("scan");

        var cancellation = new CancellationTokenSource(_remoteTimeout);
        using var remote = new RemoteFunction<ScanMessage, NotifyClientMessage>(_broker, responceUri);

        var message = new ScanMessage(barcode);
        var result = await remote.ExecuteRemoteAsync(message, requestUri, cancellation.Token);

        if (result == null)
            return new NotifyMessageDTO("Error", "Unable to get responce", NotifyTypes.Error);

        return new NotifyMessageDTO(result.Title, result.Content, NotifyTypes.Message);
    }
}

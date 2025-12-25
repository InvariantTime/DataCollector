using DataCollector.Messaging.Core.Consuming;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Forms;

namespace DataCollector.Terminal.App.Consumers;

public class NotifyConsumer : IMessageConsumer<NotifyClientMessage>
{
    private readonly IFormService _forms;

    public NotifyConsumer(IFormService forms)
    {
        _forms = forms;
    }

    public Task ConsumeAsync(ConsumeContext<NotifyClientMessage> context)
    {
        var message = new NotifyMessageDTO(context.Message.Title, context.Message.Content, NotifyTypes.Message);
        return _forms.ShowFormAsync<NotificationForm, NotifyMessageDTO>(message);
    }
}
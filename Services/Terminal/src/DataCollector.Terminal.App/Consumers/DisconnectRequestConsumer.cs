using DataCollector.Messaging.Core.Consuming;
using DataCollector.Shared.Messages;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.Services;

namespace DataCollector.Terminal.App.Consumers;

public class DisconnectRequestConsumer : IMessageConsumer<DisconnectRequestMessage>
{
    private readonly IRegisterService _register;
    private readonly IFormService _forms;

    public DisconnectRequestConsumer(IRegisterService register, IFormService forms)
    {
        _register = register;
        _forms = forms;
    }

    public async Task ConsumeAsync(ConsumeContext<DisconnectRequestMessage> context)
    {
        await _register.DisconnectAsync(false);

        await _forms.ShowFormAsync<NotificationForm, NotifyMessageDTO>(
            new NotifyMessageDTO("Disconnect", context.Message.Reason, NotifyTypes.Message));
    }
}

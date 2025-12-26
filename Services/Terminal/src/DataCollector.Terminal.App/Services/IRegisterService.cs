using DataCollector.Shared;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public interface IRegisterService
{
    Task<Result> RegisterAsync(RegisterDTO dto, CancellationToken cancellation);

    Task DisconnectAsync(bool needNotifyServer = true);
}

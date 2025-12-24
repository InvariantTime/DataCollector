using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public class ScanninService : IScanningService
{
    public Task<NotifyMessageDTO> AddProductAsync(AddProductDTO dto)
    {
        return Task.FromResult(new NotifyMessageDTO("Title", "Content", NotifyTypes.Message));
    }

    public Task<NotifyMessageDTO> ScanAsync(string barcode)
    {
        return Task.FromResult(new NotifyMessageDTO("Message", barcode, NotifyTypes.Error));
    }
}

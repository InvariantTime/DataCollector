using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Services;

public interface IScanningService
{
    Task<NotifyMessageDTO> ScanAsync(string barcode);

    Task<NotifyMessageDTO> AddProductAsync(AddProductDTO dto);
}

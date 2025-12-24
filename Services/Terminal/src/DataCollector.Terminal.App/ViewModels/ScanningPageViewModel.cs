using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.Pages;
using DataCollector.Terminal.App.Services;

namespace DataCollector.Terminal.App.ViewModels;

public partial class ScanningPageViewModel : ViewModel
{
    private readonly IScanningService _scanning;
    private readonly IFormService _forms;

    public IAsyncCommand<string> ScanCommand => 
        field ??= AsyncCommand.Create<string>(ScanAsync);

    public IAsyncCommand BackCommand =>
        field ??= AsyncCommand.Create(BackAsync);

    public ScanningPageViewModel(IScanningService scanning, IFormService forms)
    {
        _scanning = scanning;
        _forms = forms;
    }

    private async Task ScanAsync(string barcode)
    {
        var result = await _scanning.ScanAsync(barcode);
        await _forms.ShowFormAsync<NotificationForm, NotifyMessageDTO>(result);
    }

    private Task BackAsync()
    {
        return NavigateAsync<PickPage>();
    }
}

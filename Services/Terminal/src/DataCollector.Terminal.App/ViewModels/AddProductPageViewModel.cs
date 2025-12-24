using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.Pages;
using DataCollector.Terminal.App.Services;
using DataCollector.Terminal.App.ViewModels;

namespace DataCollector.Terminal.App.MVVM;

public partial class AddProductPageViewModel : ViewModel
{
    private readonly IScanningService _scanning;
    private readonly IFormService _form;

    public IAsyncCommand<string> AddProductCommand => 
        field ??= AsyncCommand.Create<string>(AddProductAsync);

    public IAsyncCommand NavigateBackCommand => 
        field ??= AsyncCommand.Create(NavigateBackAsync);

    public AddProductPageViewModel(IScanningService scanning, IFormService form)
    {
        _scanning = scanning;
        _form = form;
    }

    private async Task AddProductAsync(string barcode)
    {
        var product = await _form.ShowFormAsync<AddProductForm, AddProductDTO, string>(barcode);
        var message = await _scanning.AddProductAsync(product);

        await _form.ShowFormAsync<NotificationForm, NotifyMessageDTO>(message);
    }

    private Task NavigateBackAsync()
    {
        return NavigateAsync<PickPage>();
    }
}

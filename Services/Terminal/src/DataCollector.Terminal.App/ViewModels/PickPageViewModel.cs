using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.Pages;

namespace DataCollector.Terminal.App.ViewModels;

public partial class PickPageViewModel : ViewModel
{
    public bool CanAddProduct { get; } = true;

    public bool CanUseAdminPanel { get; } = true;

    public IAsyncCommand GoToScanningCommand => 
        field ??= AsyncCommand.Create(NavigateToScanningAsync);

    public IAsyncCommand GoToAddProductCommand => 
        field ??= AsyncCommand.Create(NavigateToAddProductAsync);

    public IAsyncCommand GoToAdminPageCommand =>
        field ??= AsyncCommand.Create(NavigateToAdminPageAsync);

    private Task NavigateToScanningAsync()
    {
        return NavigateAsync<ScanningPage>();
    }

    private Task NavigateToAddProductAsync()
    {
        if (CanAddProduct == false)
            return Task.CompletedTask;

        return NavigateAsync<AddProductPage>();
    }

    private Task NavigateToAdminPageAsync()
    {
        if (CanUseAdminPanel == false)
            return Task.CompletedTask;

        return NavigateAsync<AdminPage>();
    }
}
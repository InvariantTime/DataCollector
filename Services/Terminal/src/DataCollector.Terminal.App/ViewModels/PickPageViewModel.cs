using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.Pages;
using DataCollector.Terminal.App.Services;
using System.ComponentModel;

namespace DataCollector.Terminal.App.ViewModels;

public partial class PickPageViewModel : ViewModel
{
    private readonly ISessionProvider _session;

    public bool CanAddProduct => _session.Session?.CanAddProduct ?? false;

    public bool CanUseAdminPanel => _session.Session?.CanUseAdminPage ?? false;

    public IAsyncCommand GoToScanningCommand => 
        field ??= AsyncCommand.Create(NavigateToScanningAsync);

    public IAsyncCommand GoToAddProductCommand => 
        field ??= AsyncCommand.Create(NavigateToAddProductAsync);

    public IAsyncCommand GoToAdminPageCommand =>
        field ??= AsyncCommand.Create(NavigateToAdminPageAsync);

    public PickPageViewModel(ISessionProvider session)
    {
        _session = session;

        if (_session is INotifyPropertyChanged changed)
            changed.PropertyChanged += OnSessionChanged;
    }

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

    private void OnSessionChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanAddProduct));
        OnPropertyChanged(nameof(CanUseAdminPanel));
    }
}
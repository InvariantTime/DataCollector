using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.DTOs;
using DataCollector.Terminal.App.Pages;
using DataCollector.Terminal.App.Services;

namespace DataCollector.Terminal.App.ViewModels;

public partial class RegisterPageViewModel : ViewModel
{
    private static readonly TimeSpan _registerTimeout = TimeSpan.FromSeconds(5);

    private readonly IRegisterService _register;

    public IAsyncCommand<RegisterDTO> SubmitCommand =>
        field ??= AsyncCommand.Create<RegisterDTO>(SubmitAsync);

    public string? Error { get; private set; }

    public bool HasError { get; private set; }

    public bool IsProcessing { get; private set;  }

    public RegisterPageViewModel(IRegisterService register)
    {
        _register = register;
    }

    private async Task SubmitAsync(RegisterDTO dto)
    {
        SetProcessing(true);

        var cancellation = new CancellationTokenSource(_registerTimeout);
        var result = await _register.RegisterAsync(dto, cancellation.Token);

        if (result.IsSuccess == false)
        {
            SetError(result.Error);
        }

        SetProcessing(false);

        if (result.IsSuccess == true)
            await NavigateAsync<PickPage>();
    }

    private void SetError(string? error)
    {
        Error = null;
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(HasError));
    }

    private void ClearError()
    {
        SetError(null);
    }

    private void SetProcessing(bool value)
    {
        IsProcessing = value;
        OnPropertyChanged(nameof(IsProcessing));
    }
}

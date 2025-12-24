using DataCollector.Terminal.App.Commands;
using DataCollector.Terminal.App.Pages;

namespace DataCollector.Terminal.App.ViewModels;

public partial class RegisterPageViewModel : ViewModel
{
    public IAsyncCommand SubmitCommand
    {
        get
        {
            return field ??= AsyncCommand.Create(SubmitAsync);
        }
    }

    private Task SubmitAsync()
    {
        return NavigateAsync<PickPage>();
    }
}

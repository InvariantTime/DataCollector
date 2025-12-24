using DataCollector.Terminal.App.ViewModels;

namespace DataCollector.Terminal.App.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
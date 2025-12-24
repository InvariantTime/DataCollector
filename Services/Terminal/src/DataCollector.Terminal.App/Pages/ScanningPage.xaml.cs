
using DataCollector.Terminal.App.ViewModels;

namespace DataCollector.Terminal.App.Pages;

public partial class ScanningPage : ContentPage
{
    public ScanningPage(ScanningPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
using DataCollector.Terminal.App.MVVM;

namespace DataCollector.Terminal.App.Pages;

public partial class AddProductPage : ContentPage
{
    public AddProductPage(AddProductPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
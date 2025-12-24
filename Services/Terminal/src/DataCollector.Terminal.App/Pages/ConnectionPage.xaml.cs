using DataCollector.Terminal.App.ViewModels;

namespace DataCollector.Terminal.App.Pages;

public partial class ConnectionPage : ContentPage
{

    
	public ConnectionPage(ConnectionPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

   
}

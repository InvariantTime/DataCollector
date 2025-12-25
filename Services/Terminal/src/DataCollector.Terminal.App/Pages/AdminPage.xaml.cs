using CommunityToolkit.Maui.Extensions;

using DataCollector.Terminal.App.Controls;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.ViewModels;

namespace DataCollector.Terminal.App.Pages;

public partial class AdminPage : ContentPage
{

    

    

    public AdminPage(AdminPageViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
    
}


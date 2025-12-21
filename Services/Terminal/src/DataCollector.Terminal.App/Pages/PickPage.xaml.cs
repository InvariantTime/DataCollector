
using DataCollector.Terminal.App.Pages;

using Microsoft.Maui.Controls;

namespace DataCollector.Terminal.App.Pages;

public partial class PickPage : ContentPage
{
    public PickPage()
    {
        InitializeComponent();
    }

    public void NavigateToScanning(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Scanning());
    }

    public void NavigateToAdminPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AdminPage());
    }
    public void NavigateToAddRpoduct(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddProduct());
    }




}
using DataCollector.Terminal.App.Pages;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace DataCollector.Terminal.App.Pages;

public partial class Registration : ContentPage
{
    public Registration()
    {
        InitializeComponent();
    }

    public void OnSubMit(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PickPage());
    }

}

using ZXing.Net.Maui;

namespace DataCollector.Terminal.App.Pages;


public partial class Scanning : ContentPage
{


    public Scanning()
    {
        InitializeComponent();

    }

    async void AlertButton_Clicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Error 404", "Back", null, "Error потому что ты дибил");

    }

    public void ScanningQR(object sender, EventArgs e)
    {

    }

    public void NavigationToPickPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PickPage());
    }





}
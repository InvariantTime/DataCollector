using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Controls;

public partial class MessageWindow : Popup
{


    public MessageWindow()
    {
        InitializeComponent();
    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }




}
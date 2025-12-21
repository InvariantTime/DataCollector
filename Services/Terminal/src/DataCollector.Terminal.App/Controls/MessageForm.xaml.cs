using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Controls;

public partial class MessageForm : Popup
{
    public MessageForm()
    {
        InitializeComponent();
    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }

    public void OnSendMessageWorkers(object sender, EventArgs e)
    {

    }

}
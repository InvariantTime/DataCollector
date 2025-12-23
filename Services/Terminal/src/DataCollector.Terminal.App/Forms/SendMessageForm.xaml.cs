using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Forms;

public partial class SendMessageForm : Popup
{
    public SendMessageForm()
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
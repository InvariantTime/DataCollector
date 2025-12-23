

using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Forms;

public partial class AdminActionForm : Popup
{
    public AdminActionForm()
    {
        InitializeComponent();
    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }

    public void OnMessageWorkers(object sender, EventArgs e)
    {
    }

    public void OnKikWorker(object sender, EventArgs e)
    {

    }

    public void OnSetRoleWorker(object sender, EventArgs e)
    {

    }
}
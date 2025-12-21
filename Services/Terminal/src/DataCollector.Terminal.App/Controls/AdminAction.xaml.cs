

using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Controls;

public partial class AdminAction : Popup
{
    private readonly Page _parentPage;

    public AdminAction(Page parentPage)
    {
        InitializeComponent();
        _parentPage = parentPage;
    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }

    public void OnMessageWorkers(object sender, EventArgs e)
    {
        OnCloseAsync(sender, e);
        _parentPage.ShowPopupAsync(new MessageForm());

    }

    public void OnKikWorker(object sender, EventArgs e)
    {

    }

    public void OnSetRoleWorker(object sender, EventArgs e)
    {

    }
}
using CommunityToolkit.Maui.Views;
using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Forms;

public partial class NotificationForm : Popup, IWithParameter<NotifyMessageDTO>
{
    public NotificationForm()
    {
        InitializeComponent();
    }

    public Task InitializeParameterAsync(NotifyMessageDTO parameter)
    {
        _title.Text = parameter.Title;
        _description.Text = parameter.Content;

        return Task.CompletedTask;
    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }
}


using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Controls;

public partial class AddProductForm : Popup
{
    public AddProductForm()
    {
        InitializeComponent();
    }

    public void AddProduct(object sender, EventArgs e)
    {

    }

    public void OnCloseAsync(object sender, EventArgs e)
    {
        CloseAsync();
    }
}
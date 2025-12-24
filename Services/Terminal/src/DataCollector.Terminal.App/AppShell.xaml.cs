using DataCollector.Terminal.App.Pages;

namespace DataCollector.Terminal.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        RegisterRoute<ConnectionPage>();
        RegisterRoute<RegisterPage>();
        RegisterRoute<PickPage>();
        RegisterRoute<ScanningPage>();
        RegisterRoute<AddProductPage>();
        RegisterRoute<AdminPage>();
    }

    private static void RegisterRoute<T>() where T : Page
    {
        var type = typeof(T);
        Routing.RegisterRoute(type.FullName, typeof(T));
    }
}

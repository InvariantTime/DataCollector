using DataCollector.Terminal.App.Services;

namespace DataCollector.Terminal.App;

public partial class App : Application
{
    private readonly BackgroundServiceStartup _statup;

    public App(BackgroundServiceStartup startup)
    {
        InitializeComponent();

        _statup = startup;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnStart()
    {
        _statup.StartAsync()
            .GetAwaiter().GetResult();
    }
}
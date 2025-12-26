using Android.App;
using Android.Content.PM;
using Android.OS;
using DataCollector.Terminal.App.Services;

namespace DataCollector.Terminal.App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnDestroy()
    {
        if (IPlatformApplication.Current == null)
            return;

        var startup = IPlatformApplication.Current.Services.GetService<BackgroundServiceStartup>();
        startup?.StopAsync();

        base.OnDestroy();
    }
}

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using DataCollector.Messaging.DI;
using ZXing.Net.Maui.Controls;
using DataCollector.Messaging.MQTT;

namespace DataCollector.Terminal.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddRequiredServices();
        builder.Services.AddRequiredViewModels();
        builder.Services.AddReguiredPopups();


        builder.Services.AddMessageBroker(broker =>
        {
            broker.UseConnection<MqttConnectionProvider>();
        });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

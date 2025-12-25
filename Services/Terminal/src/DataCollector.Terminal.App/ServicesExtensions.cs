using CommunityToolkit.Maui;
using DataCollector.Terminal.App.Forms;
using DataCollector.Terminal.App.MVVM;
using DataCollector.Terminal.App.Services;
using DataCollector.Terminal.App.ViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Controls.Shapes;

namespace DataCollector.Terminal.App;

public static class ServicesExtensions
{
    public static void AddRequiredServices(this IServiceCollection services)
    {
        services.AddSingleton<IScanningService, ScanninService>();
        services.AddSingleton<ISessionProvider, SessionProvider>();
        services.AddSingleton<IRegisterService, RegisterService>();
        services.AddSingleton<IAdminService, AdminService>();
    }

    public static void AddRequiredViewModels(this IServiceCollection services)
    {
        services.AddSingleton<RegisterPageViewModel>();
        services.AddSingleton<PickPageViewModel>();
        services.AddSingleton<ScanningPageViewModel>();
        services.AddSingleton<AddProductPageViewModel>();
        services.TryAddSingleton<ConnectionPageViewModel>();
    }

    public static void AddReguiredPopups(this IServiceCollection services)
    {
        services.AddFormService((builder) =>
        {
            builder.AddForm<NotificationForm>()
                .AddForm<AddProductForm>()
                .AddForm<AdminActionForm>()
                .AddForm<SendMessageForm>();

            builder.SetPopupOptions(new PopupOptions
            {
                CanBeDismissedByTappingOutsideOfPopup = false,
                Shape = new RoundRectangle
                {
                    BackgroundColor = Color.FromArgb("#242427"),
                    Stroke = Color.FromArgb("#303035"),
                    StrokeThickness = 3
                }
            });
        });
    }
}

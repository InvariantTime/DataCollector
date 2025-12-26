using CommunityToolkit.Maui;

namespace DataCollector.Terminal.App.Forms;

public static class FormServicesExtensions
{
    public static void AddFormService(this IServiceCollection services, Action<FormServiceBuilder>? buildAction = null)
    {
        var builder = new FormServiceBuilder();
        buildAction?.Invoke(builder);

        builder.Build(services);
    }
}

public class FormServiceBuilder
{
    private readonly List<Type> _forms = new();
    private IPopupOptions? _options;

    public FormServiceBuilder AddForm<T>() where T : ContentView
    {
        _forms.Add(typeof(T));
        return this;
    }

    public void SetPopupOptions(IPopupOptions options)
    {
        _options = options;
    }

    public void Build(IServiceCollection services)
    {
        foreach (var form in _forms)
            services.AddSingleton(form);

        services.AddSingleton<IFormService>(scope =>
        {
            var forms = _forms.Select(x => scope.GetRequiredService(x)).Cast<ContentView>();
            var dispatcher = scope.GetRequiredService<IDispatcher>();

            var service = new FormService(forms, _options ?? PopupOptions.Empty, dispatcher);

            return service;
        });
    }
}

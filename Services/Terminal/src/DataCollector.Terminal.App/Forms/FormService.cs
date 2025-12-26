using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using System.Collections.Immutable;

namespace DataCollector.Terminal.App.Forms;

public class FormService : IFormService
{
    private readonly IPopupOptions _popupOptions;
    private readonly ImmutableDictionary<Type, ContentView> _forms;
    private readonly IDispatcher _dispatcher;

    public FormService(IEnumerable<ContentView> forms, IPopupOptions options, IDispatcher dispatcher)
    {
        _forms = forms.ToImmutableDictionary(x => x.GetType());
        _popupOptions = options;
        _dispatcher = dispatcher;
    }

    public Task ShowFormAsync<T>(CancellationToken cancellation = default) where T : ContentView
    {
        if (_forms.TryGetValue(typeof(T), out var form) == false)
            throw new InvalidOperationException($"Unable to get form {typeof(T)}");

        return OpenPopupAsync(form, cancellation);
    }

    public async Task<TResult> ShowFormAsync<TForm, TResult>(CancellationToken cancellation = default)
        where TForm : ContentView, IWithResult<TResult>
    {
        if (_forms.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await OpenPopupAsync(form, cancellation);

        return await form.AwaitResultAsync(cancellation);
    }

    public async Task ShowFormAsync<TForm, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : ContentView, IWithParameter<TParameter>
    {
        if (_forms.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await form.InitializeParameterAsync(parameter);
        await OpenPopupAsync(form, cancellation);
    }

    public async Task<TResult> ShowFormAsync<TForm, TResult, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : ContentView, IWithParameter<TParameter>, IWithResult<TResult>
    {
        if (_forms.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await form.InitializeParameterAsync(parameter);
        await OpenPopupAsync(form, cancellation);

        return await form.AwaitResultAsync(cancellation);
    }

    private Task OpenPopupAsync(ContentView form, CancellationToken cancellation)
    {
        if (form is IClosable closable)
            closable.CloseAsyncCallback = ClosePopupAsync;

        if (_dispatcher.IsDispatchRequired == false)
            return Shell.Current.ShowPopupAsync(form, _popupOptions, cancellation);

        return _dispatcher.DispatchAsync(() =>
        {
            return Shell.Current.ShowPopupAsync(form, _popupOptions, cancellation);
        });
    }

    private static Task ClosePopupAsync()
    {
        return Shell.Current.ClosePopupAsync();
    }
}

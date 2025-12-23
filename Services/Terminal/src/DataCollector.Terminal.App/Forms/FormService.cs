using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using System.Collections.Immutable;

namespace DataCollector.Terminal.App.Forms;

public class FormService : IFormService
{
    private readonly ImmutableDictionary<Type, Popup> _popups;

    public FormService(IEnumerable<Popup> popups)
    {
        _popups = popups.ToImmutableDictionary(x => x.GetType());
    }

    public Task ShowFormAsync<T>(CancellationToken cancellation = default) where T : Popup
    {
        if (_popups.TryGetValue(typeof(T), out var form) == false)
            throw new InvalidOperationException($"Unable to get form {typeof(T)}");

        return OpenPopupAsync(form, cancellation);
    }

    public async Task<TResult> ShowFormAsync<TForm, TResult>(CancellationToken cancellation = default)
        where TForm : Popup, IWithResult<TResult>
    {
        if (_popups.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await OpenPopupAsync(form, cancellation);

        return await form.AwaitResultAsync(cancellation);
    }

    public async Task ShowFormAsync<TForm, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : Popup, IWithParameter<TParameter>
    {
        if (_popups.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await form.InitializeParameterAsync(parameter);
        await OpenPopupAsync(form, cancellation);
    }

    public async Task<TResult> ShowFormAsync<TForm, TResult, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : Popup, IWithParameter<TParameter>, IWithResult<TResult>
    {
        if (_popups.TryGetValue(typeof(TForm), out var res) == false || res is not TForm form)
            throw new InvalidOperationException($"Unable to get form {typeof(TForm)}");

        await form.InitializeParameterAsync(parameter);
        await OpenPopupAsync(form, cancellation);

        return await form.AwaitResultAsync(cancellation);
    }

    private Task OpenPopupAsync(Popup popup, CancellationToken cancellation)
    {
        var page = Shell.Current.CurrentPage;
        return page.ShowPopupAsync(popup, token: cancellation);
    }
}

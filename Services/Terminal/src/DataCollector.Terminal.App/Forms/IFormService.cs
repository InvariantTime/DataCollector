using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Forms;

public interface IFormService
{
    Task ShowFormAsync<T>(CancellationToken cancellation = default) where T : Popup;

    Task<TResult> ShowFormAsync<TForm, TResult>(CancellationToken cancellation = default) where TForm : Popup, IWithResult<TResult>;

    Task ShowFormAsync<TForm, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : Popup, IWithParameter<TParameter>;

    Task<TResult> ShowFormAsync<TForm, TResult, TParameter>(TParameter parameter, CancellationToken cancellation = default) 
        where TForm : Popup, IWithParameter<TParameter>, IWithResult<TResult>;
}
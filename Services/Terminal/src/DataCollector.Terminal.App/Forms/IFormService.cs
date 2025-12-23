using CommunityToolkit.Maui.Views;

namespace DataCollector.Terminal.App.Forms;

public interface IFormService
{
    Task ShowFormAsync<T>(CancellationToken cancellation = default) where T : ContentView;

    Task<TResult> ShowFormAsync<TForm, TResult>(CancellationToken cancellation = default) where TForm : ContentView, IWithResult<TResult>;

    Task ShowFormAsync<TForm, TParameter>(TParameter parameter, CancellationToken cancellation = default)
        where TForm : ContentView, IWithParameter<TParameter>;

    Task<TResult> ShowFormAsync<TForm, TResult, TParameter>(TParameter parameter, CancellationToken cancellation = default) 
        where TForm : ContentView, IWithParameter<TParameter>, IWithResult<TResult>;
}
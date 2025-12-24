using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Forms;

public partial class AddProductForm : ContentView, IWithResult<AddProductDTO>, IWithParameter<string>, IClosable
{
    private TaskCompletionSource<AddProductDTO> _tcs;

    public Func<Task>? CloseAsyncCallback { get; set; }

    public string? BarCode { get; private set; }
 
    public AddProductForm()
    {
        InitializeComponent();
        _tcs = new();
    }

    public async Task<AddProductDTO> AwaitResultAsync(CancellationToken cancellation)
    {
        var result = await _tcs.Task.WaitAsync(cancellation);
        _tcs = new TaskCompletionSource<AddProductDTO>();

        return result;
    }

    public Task InitializeParameterAsync(string parameter)
    {
        BarCode = parameter;
        _title.Text = parameter;

        return Task.CompletedTask;
    }

    private async void OnClosedAsync(object sender, EventArgs e)
    {
        var task = CloseAsyncCallback?.Invoke() ?? Task.CompletedTask;
        await task;
    }

    private void OnAddProduct(object sender, EventArgs e)
    {
        var name = _nameTextbox.Text;
        var description = _descriptionTextbox.Text;

        var product = new AddProductDTO(BarCode ?? string.Empty, name, description);
        _tcs.TrySetResult(product);
    }
}
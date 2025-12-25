using DataCollector.Terminal.App.DTOs;

namespace DataCollector.Terminal.App.Forms;

public partial class AddProductForm : ContentView, IWithResult<AddProductDTO>, IWithParameter<string>, IClosable
{
    public static readonly BindableProperty BarcodeProperty = 
        BindableProperty.Create(nameof(Barcode), typeof(string), typeof(AddProductForm), string.Empty, BindingMode.OneWay);

    private TaskCompletionSource<AddProductDTO> _tcs;

    public Func<Task>? CloseAsyncCallback { get; set; }

    public string Barcode
    {
        get
        {
            return (string)GetValue(BarcodeProperty);
        }

        set
        {
            SetValue(BarcodeProperty, value);
        }
    }
 
    public AddProductForm()
    {
        InitializeComponent();
        _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public async Task<AddProductDTO> AwaitResultAsync(CancellationToken cancellation)//TODO: Make common system for waiting for result
    {
        var result = await _tcs.Task.WaitAsync(cancellation);
        _tcs = new TaskCompletionSource<AddProductDTO>(TaskCreationOptions.RunContinuationsAsynchronously);

        return result;
    }

    public Task InitializeParameterAsync(string parameter)
    {
        Barcode = parameter;
        return Task.CompletedTask;
    }

    private void OnClosedAsync(object sender, EventArgs e)
    {
        var task = CloseAsyncCallback?.Invoke() ?? Task.CompletedTask;
        task.GetAwaiter().GetResult();
    }

    private void OnAddProduct(object sender, EventArgs e)
    {
        var name = _nameTextbox.Text;
        var description = _descriptionTextbox.Text;

        var product = new AddProductDTO(Barcode ?? string.Empty, name, description);
        _tcs.TrySetResult(product);
        CloseAsyncCallback?.Invoke();
    }
}
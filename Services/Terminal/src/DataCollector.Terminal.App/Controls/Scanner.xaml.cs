using System.Timers;
using ZXing.Net.Maui;

namespace DataCollector.Terminal.App.Controls;

public partial class Scanner : ContentView
{
    private static readonly TimeSpan _timerInterval = TimeSpan.FromSeconds(1.5);

    public static readonly BindableProperty IsDetectedProperty = 
        BindableProperty.Create(nameof(IsDetected), typeof(bool), typeof(Scanner), false, BindingMode.OneWay);

    public static readonly BindableProperty BarcodeProperty =
        BindableProperty.Create(nameof(Barcode), typeof(string), typeof(Scanner), string.Empty, BindingMode.OneWay);

    private readonly IDispatcherTimer _disableTimer;

    public string Barcode
    {
        get
        {
            return (string)GetValue(BarcodeProperty);
        }

        private set
        {
            SetValue(BarcodeProperty, value);
        }
    }

    public bool IsDetected
    {
        get
        {
            return (bool)GetValue(IsDetectedProperty);
        }

        private set
        {
            SetValue(IsDetectedProperty, value);
        }
    }

	public Scanner()
	{
		InitializeComponent();

        _camera.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true,
            TryHarder = false,
        };

        _camera.IsDetecting = true;
        _camera.IsTorchOn = true;

        _disableTimer = Dispatcher.CreateTimer();
        _disableTimer.Interval = _timerInterval;
        _disableTimer.Tick += OnTimerElapsed;
	}

    private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var result = e.Results.FirstOrDefault();

        if (result == null)
            return;

        _disableTimer.Stop();
        _disableTimer.Start();

        IsDetected = true;
        Barcode = result.Value;
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    {
        if (IsDetected == false)
            return;

        IsDetected = false;
        Barcode = string.Empty;
    }
}
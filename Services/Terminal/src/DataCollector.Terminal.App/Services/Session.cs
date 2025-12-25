using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCollector.Terminal.App.Services;

public class Session : INotifyPropertyChanged, IDisposable
{
    private readonly IDisposable? _holder;

    private readonly ConcurrentDictionary<string, Uri> _uriRequestMapper;
    private readonly ConcurrentDictionary<string, Uri> _uriResponceMapper;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Guid Id { get; }

    public bool CanAddProduct { get; private set; }

    public bool CanUseAdminPage { get; private set; }

    public Session(Guid id, IDisposable? sessionHolder = null)
    {
        Id = id;
        _uriRequestMapper = new ConcurrentDictionary<string, Uri>();
        _uriResponceMapper = new ConcurrentDictionary<string, Uri>();
        _holder = sessionHolder;
    }

    public void SetCanAddProduct(bool value)
    {
        CanAddProduct = value;
        OnPropertyChanged(nameof(CanAddProduct));
    }

    public void SetCanUseAdminPage(bool value)
    {
        CanUseAdminPage = value;
        OnPropertyChanged(nameof(CanUseAdminPage));
    }

    public Uri CreateRequestUri(string message)
    {
        var result = _uriRequestMapper.GetOrAdd(message, (value) =>
        {
            return new Uri($"mqtt:devices/{Id}/request/{value}");
        });

        return result;
    }

    public Uri CreateResponceUri(string message)
    {
        var result = _uriResponceMapper.GetOrAdd(message, (value) =>
        {
            return new Uri($"mqtt:devices/{Id}/responce/{value}");
        });

        return result;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public void Dispose()
    {
        _holder?.Dispose();
        _uriRequestMapper.Clear();
        _uriResponceMapper.Clear();
    }
}

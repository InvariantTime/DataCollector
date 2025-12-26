namespace DataCollector.Shared;

public struct DisposeBuilder : IDisposable
{
    private object? _disposable;

    public void AddDisposable(IDisposable disposable)
    {
        if (_disposable == null)
        {
            _disposable = disposable;
        }
        else if (_disposable is IDisposable other)
        {
            _disposable = new List<IDisposable>()
            {
               other,
               disposable
            };
        }
        else if (_disposable is List<IDisposable> list)
        {
            list.Add(disposable);
        }
    }

    public void Dispose()
    {
        if (_disposable is IDisposable disposable)
        {
            disposable.Dispose();
        }
        else if (_disposable is List<IDisposable> list)
        {
            foreach (var dis in list)
                dis.Dispose();
        }
    }
}

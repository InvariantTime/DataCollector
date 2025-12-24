using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCollector.Terminal.App.ViewModels;

public abstract partial class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Task NavigateAsync<T>() where T : Page
    {
        var type = typeof(T);
        return Shell.Current.GoToAsync(type.FullName);
    }

    public void OnPropertyChanged([CallerMemberName]string? property = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}

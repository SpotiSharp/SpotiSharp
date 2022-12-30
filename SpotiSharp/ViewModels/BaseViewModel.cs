using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotiSharp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    internal delegate void VisibilityChange();
    public event PropertyChangedEventHandler PropertyChanged;

    internal event VisibilityChange OnVisibilityChange;
    
    internal bool isVisible = false;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    internal virtual void OnAppearing()
    {
        isVisible = true;
        OnVisibilityChange?.Invoke();
    }

    internal virtual void OnDisappearing()
    {
        isVisible = false;
        OnVisibilityChange?.Invoke();
    }
}
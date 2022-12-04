using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class SongsListViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private List<Song> _songs;

    public List<Song> Songs
    {
        get { return _songs; }
        set { SetProperty(ref _songs, value); }
    }

    public void OnPlayListIdRefresh(string playlistId)
    {
        var songsListModel = new SongsListModel(playlistId);
        Songs = songsListModel.Songs;
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
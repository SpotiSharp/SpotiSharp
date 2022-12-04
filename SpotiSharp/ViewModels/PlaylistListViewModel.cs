using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpotifyAPI;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class PlaylistListViewModel : INotifyPropertyChanged
{
    private List<Playlist> _userRelatedPLayLists;

    public List<Playlist> UserRelatedPLayLists
    {
        get { return _userRelatedPLayLists; }
        set { SetProperty(ref _userRelatedPLayLists, value); }
    }

    public PlaylistListViewModel()
    {
        var playlistListModel = new PlaylistListModel();

        UserRelatedPLayLists = playlistListModel.PlayLists;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

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
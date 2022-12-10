using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class PlaylistListViewModel : INotifyPropertyChanged
{
    private Playlist _selectedPlaylist;

    public Playlist SelectedPlaylist
    {
        get { return _selectedPlaylist; }
        set { SetProperty(ref _selectedPlaylist, value); }
    }

    private List<Playlist> _playLists;

    public List<Playlist> PlayLists
    {
        get { return _playLists; }
        set { SetProperty(ref _playLists, value); }
    }

    public PlaylistListViewModel()
    {
        var playlistListModel = new PlaylistListModel();
        PlayLists = playlistListModel.PlayLists;
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

    public async void GoToPlaylistDetail()
    {
        if (SelectedPlaylist == null) return;
        string playlistId = SelectedPlaylist.PlayListId;
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "PlaylistId",  playlistId}
        };

        await Shell.Current.GoToAsync($"DetailPlaylistPage", navigationParameter);
    }
}
using System.Windows.Input;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class PlaylistCreationSonglistViewModel : BaseViewModel
{
    private List<Song> _songs = new List<Song>();

    public List<Song> Songs
    {
        get { return _songs; }
        set { SetProperty(ref _songs, value); }
    }

    public PlaylistCreationSonglistViewModel()
    {
        RemoveSong = new Command(RemoveSongHandler);
        
        PlaylistCreatorPageModel.OnSongListChange += RefreshSongs;
    }

    private async void RefreshSongs()
    {
        Songs = (await PlaylistCreatorPageModel.GetFilteredSongs()).Select((fullTrack, index) =>
        {
            return new SongEditable(index, fullTrack);
        }).ToList();
        OnPlalistIsFiltered?.Invoke();
    }
    
    private void RemoveSongHandler(object obj)
    {
        var index = 0;
        PlaylistCreatorPageModel.RemoveSongByIndex(index);
    }

    public ICommand RemoveSong { private set; get; }
}
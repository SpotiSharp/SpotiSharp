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

    private void RefreshSongs()
    {
        Songs = (from fullTrack in PlaylistCreatorPageModel.GetFilteredSongs() select new Song(fullTrack)).ToList();
    }
    
    private void RemoveSongHandler(object obj)
    {
        var index = 0;
        PlaylistCreatorPageModel.RemoveSongByIndex(index);
    }

    public ICommand RemoveSong { private set; get; }
}
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class SongsListViewModel : BaseViewModel
{
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

    public void ClickSong(object sourceItem)
    {
        if (sourceItem is Song song) SpotifyAPI.APICaller.Instance?.SetCurrentPlayingSong(song.SongId, song.PartOfPlayListWithId);
    }
}
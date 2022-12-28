using SpotifyAPI;
using SpotifyAPI.Web;
using SpotiSharp.Interfaces;

namespace SpotiSharp.Models;

public delegate void SongsChange();
public static class PlaylistCreatorPageModel
{
    private static List<FullTrack> _unfilteredSongs = new List<FullTrack>();

    private static List<FullTrack> UnfilteredSongs
    {
        get
        {
            return _unfilteredSongs;
        }
        set
        {
            OnSongListChange?.Invoke(); 
            _unfilteredSongs = value;
        }
    }

    public static List<IFilterViewModel> Filters { get; set; } = new List<IFilterViewModel>();

    public static event SongsChange OnSongListChange;

    public static void AddSong(string songId)
    {
        FullTrack? song = APICaller.Instance?.GetTrackById(songId);
        var tmpSongs = UnfilteredSongs;
        if (song != null) tmpSongs.Add(song);
        UnfilteredSongs = tmpSongs;
    }
    
    public static void AddSongsFromPlaylist(string playlistId)
    {
        Paging<PlaylistTrack<IPlayableItem>>? songs = APICaller.Instance?.GetTracksByPlaylistId(playlistId);
        var tmpSongs = UnfilteredSongs;
        if (songs?.Items == null) return;
        foreach (var playable in songs.Items)
        {
            if (playable.Track is FullTrack song)
            {
                tmpSongs.Add(song);
            }
        }

        UnfilteredSongs = tmpSongs;
    }

    public static void RemoveSongByIndex(int index)
    {
        var tmpSongs = UnfilteredSongs;
        tmpSongs.RemoveAt(index);
        UnfilteredSongs = tmpSongs;
    }
    
    public static List<FullTrack> GetFilteredSongs()
    {
        var resultSongs = UnfilteredSongs;
        foreach (var filter in Filters)
        {
            resultSongs = filter.FilterSongs(resultSongs);
        }

        return resultSongs;
    }
}
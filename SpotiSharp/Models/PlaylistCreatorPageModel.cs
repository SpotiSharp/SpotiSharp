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

    public static void RemoveSongsByIndex(List<int> indexes)
    {
        indexes.Sort();
        indexes.Reverse();
        var tmpSongs = UnfilteredSongs;
        foreach (var index in indexes)
        {
            tmpSongs.RemoveAt(index);
        }
        
        UnfilteredSongs = tmpSongs;
    }
    
    public async static Task<List<FullTrack>> GetFilteredSongs()
    {
        var resultSongs = UnfilteredSongs;
        
        foreach (var filter in Filters)
        {
            var apiCallerInstance = await APICaller.WaitForRateLimitWindowInstance;
            List<TrackAudioFeatures>? currentAudioFeatures = apiCallerInstance?.GetMultipleTrackAudioFeaturesByTrackIds(resultSongs.Select(rs => rs.Id).ToList());
            if (currentAudioFeatures != null) resultSongs = await filter.FilterSongs(resultSongs, currentAudioFeatures);
        }

        return resultSongs;
    }

    public static void ApplyFilters()
    {
        OnSongListChange?.Invoke();
    }
}
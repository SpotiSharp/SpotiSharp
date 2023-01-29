using SpotiSharpBackend;
using SpotifyAPI.Web;
using SpotiSharp.Consts;
using SpotiSharp.Interfaces;

namespace SpotiSharp.Models;

public delegate void SongsChange();
public delegate void SongsUiUpdate();

public static class PlaylistCreatorPageModel
{
    public static string PlaylistName = string.Empty;

    private static List<FullTrack> _unfilteredSongs = new List<FullTrack>();

    public static List<FullTrack> UnfilteredSongs
    {
        get { return _unfilteredSongs; }
        set
        {
            _unfilteredSongs = value;
            OnSongListChange?.Invoke();
        }
    }

    public static List<FullTrack> UnfilteredSongsOnlyUiUpdate
    {
        get => _unfilteredSongs;
        set
        {
            _unfilteredSongs = value;
            OnSongsUiUpdate?.Invoke();
        }
    }

    private static List<FullTrack> _currentFilteredSongs = new List<FullTrack>();


    public static List<FullTrack> CurrentFilteredSongs
    {
        get => _currentFilteredSongs;
        set
        {
            _currentFilteredSongs = value;
            OnSongsUiUpdate?.Invoke();
        }
    }
    
    public static List<IFilterViewModel> Filters { get; set; } = new List<IFilterViewModel>();

    public static event SongsChange OnSongListChange;
    public static event SongsUiUpdate OnSongsUiUpdate;


    public static void AddSong(string songId)
    {
        if (songId == null) return;
        FullTrack? song = APICaller.Instance?.GetTrackById(songId);
        var tmpSongs = UnfilteredSongs;
        if (song != null) tmpSongs.Add(song);
        UnfilteredSongs = tmpSongs;
    }

    public static void AddSongsFromPlaylist(string playlistId)
    {
        var tmpSongs = UnfilteredSongs;

        if (playlistId == Constants.LIKED_PLALIST_ID)
        {
            tmpSongs.AddRange(APICaller.Instance?.GetUserLikedSongs().Select(ls => ls.Track).ToList() ?? new List<FullTrack>());
        }
        else
        {
            IList<PlaylistTrack<IPlayableItem>>? songs = APICaller.Instance?.GetTracksByPlaylistId(playlistId);
            if (songs == null) return;
            foreach (var playable in songs)
            {
                if (playable.Track is FullTrack song && song.Id != null)
                {
                    tmpSongs.Add(song);
                }
            }
        }

        if (StorageHandler.IsUsingCollaborationHost)
        {
            UnfilteredSongsOnlyUiUpdate = tmpSongs;
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

    public static void ClearSongs()
    {
        UnfilteredSongs = new List<FullTrack>();
    }

    public static async Task<List<FullTrack>> GetFilteredSongs()
    {
        if (StorageHandler.IsUsingCollaborationHost) return CurrentFilteredSongs;
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

    public static void CreatePlaylist()
    {
        APICaller.Instance?.CreatePlaylistWithTrackUris(PlaylistName, CurrentFilteredSongs.Select(cfs => cfs.Uri).ToList());
    }

    private static void RemoveFilter(int index)
    {
        Filters.RemoveAt(index);
    }

    public static bool SongsEqual(List<FullTrack> songs1, List<FullTrack> songs2)
    {
        if (songs1.Count != songs2.Count) return false;

        for (int i = 0; i < songs1.Count; i++)
        {
            if (!songs1[i].Id.Equals(songs2[i].Id)) return false;
        }

        return true;
    }
}
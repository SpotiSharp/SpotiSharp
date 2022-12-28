using SpotifyAPI.Web;


namespace SpotifyAPI;

public class APICaller
{
    private static readonly APICaller ApiCaller = new();

    public static APICaller? Instance
    {
        get
        {
            if (Authentication.SpotifyClient != null && Ratelimiter.CanRequestCall())
            {
                return ApiCaller;
            }

            return null;
        }
    }

    private const int MAX_RETRIES = 5;
    private const int TIME_OUT_IN_MILLI = 100;
    
    private APICaller() {}
    
    private T HandleExceptionsNonAbstract<T>(Func<T> call) where T : new()
    {
        var result = HandleExceptions(call);
        return result == null ? new T() : result;
    }

    private T? HandleExceptions<T>(Func<T> call)
    {
        int currentRetries = 0;
        while (currentRetries < MAX_RETRIES)
        {
            try
            {
                if (Ratelimiter.RequestCall()) return call();
            }
            catch (AggregateException)
            {
                currentRetries++;
                Thread.Sleep(TIME_OUT_IN_MILLI);
            }
        }

        return default;
    }


    #region PlayList

    public FullPlaylist GetPlaylistById(string playlistId)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Playlists.Get(playlistId).Result);
    }

    public Paging<SimplePlaylist> GetAllUserPlaylists()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Playlists.CurrentUsers().Result);
    }

    public Paging<PlaylistTrack<IPlayableItem>>? GetTracksByPlaylistId(string playlistId)
    {
        FullPlaylist? playlist = GetPlaylistById(playlistId);
        return playlist.Tracks ??= new Paging<PlaylistTrack<IPlayableItem>>();
    }

    public List<string> GetTrackIdsByPlaylistId(string playlistId)
    {
        Paging<PlaylistTrack<IPlayableItem>> songs = GetTracksByPlaylistId(playlistId);
        return (from song in songs.Items
            let songAsTrack = song.Track as FullTrack
            select songAsTrack.Uri).ToList();
    }

    #endregion

    #region Track

    public FullTrack GetTrackById(string trackId)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Tracks.Get(trackId).Result);
    }

    public TracksResponse GetMultipleTrackByTrackId(List<string> trackIds)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Tracks.GetSeveral(new TracksRequest(trackIds)).Result);
    }

    public TrackAudioFeatures GetAudioFeaturesByTrackId(string trackId)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Tracks.GetAudioFeatures(trackId).Result);
    }

    public TracksAudioFeaturesResponse GetMultipleAudioFeaturesByTrackIds(List<string> trackIds)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Tracks.GetSeveralAudioFeatures(new TracksAudioFeaturesRequest(trackIds)).Result);
    }

    #endregion

    #region Player

    public CurrentlyPlayingContext GetCurrentPlaybackContext()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.GetCurrentPlayback(new PlayerCurrentPlaybackRequest()).Result);
    }
    
    public CurrentlyPlaying GetCurrentSong()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result);
    }

    public bool SetCurrentPlayingSong(string songId, string playlistId)
    {
        var playlist = GetPlaylistById(playlistId);
        var songToPlay = GetTrackById(songId);

        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            ContextUri = playlist.Uri,
            OffsetParam = new PlayerResumePlaybackRequest.Offset
            {
                Uri = songToPlay.Uri
            }
        }).Result);
    }

    public bool SetCurrentPlayingToPlaylist(string playlistId)
    {
        var playlist = GetPlaylistById(playlistId);

        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            ContextUri = playlist.Uri
        }).Result);
    }

    public bool TogglePlaybackStatus()
    {
        var playContext = HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.GetCurrentPlayback().Result);
        playContext = null;
        return playContext != null && (playContext.IsPlaying
            ? HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.PausePlayback().Result)
            : HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.ResumePlayback().Result));
    }

    public bool ChangePlaybackRepeatType()
    {
        var currentlyPlayingContext = HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.GetCurrentPlayback().Result);
        if (!Enum.TryParse(currentlyPlayingContext.RepeatState, true, out PlayerSetRepeatRequest.State currentRepeatState)) return false;

        var states = Enum.GetValues<PlayerSetRepeatRequest.State>().Reverse().ToArray();
        int indexOfNextItem = Array.IndexOf(states, currentRepeatState) + 1;
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.SetRepeat(new PlayerSetRepeatRequest(states[indexOfNextItem % states.Length])).Result);
    }

    public bool TogglePlaybackShuffle()
    {
        var currentlyPlayingContext = HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.GetCurrentPlayback().Result);
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.SetShuffle(currentlyPlayingContext.ShuffleState ? new PlayerShuffleRequest(false) : new PlayerShuffleRequest(true)).Result);
    }

    public bool SkipToNextSong()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.SkipNext().Result);
    }

    public bool SkipToPreviousSong()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.SkipPrevious().Result);
    }

    public bool SetVolume(int volume)
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.Player.SetVolume(new PlayerVolumeRequest(volume)).Result);
    }

    #endregion

    #region Profile

    public string? GetUserName()
    {
        return HandleExceptions(() => Authentication.SpotifyClient.UserProfile.Current().Result.DisplayName);
    }

    public string? GetProfilePictureURL()
    {
        return HandleExceptions(() => Authentication.SpotifyClient.UserProfile.Current().Result.Images.ElementAtOrDefault(0)?.Url ?? string.Empty);
    }

    public Followers GetUserFollows()
    {
        return HandleExceptionsNonAbstract(() => Authentication.SpotifyClient.UserProfile.Current().Result.Followers);
    }

    #endregion
}
using SpotifyAPI.Web;

namespace SpotifyAPI;

public static class Player
{
    public static CurrentlyPlaying GetCurrentSong()
    {
        return Authentication.SpotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result;
    }

    public static bool SetCurrentPlayingSong(string playlistId)
    {
        Paging<PlaylistTrack<IPlayableItem>> songs = PlayList.GetTracksByPlaylistId(playlistId);
        List<string> songUris = (from song in songs.Items 
            let songAsTrack = song.Track as FullTrack 
            select songAsTrack.Uri).ToList();
        
        if (songUris.Count == 0) return false;
        return Authentication.SpotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            Uris = songUris
        }).Result;
    }
    
    public static bool TogglePlaybackStatus()
    {
        var playContext = Authentication.SpotifyClient.Player.GetCurrentPlayback().Result;
        if (playContext == null) return false;
        if (playContext.IsPlaying)
        {
            return Authentication.SpotifyClient.Player.PausePlayback().Result;
        }
        return Authentication.SpotifyClient.Player.ResumePlayback().Result;
    }
    
    public static bool ChangePlaybackRepeatType()
    {
        var currentlyPlayingContext = Authentication.SpotifyClient.Player.GetCurrentPlayback().Result;
        if (!Enum.TryParse(currentlyPlayingContext.RepeatState, out PlayerSetRepeatRequest.State currentRepeatState)) return false;
        var states = Enum.GetValues<PlayerSetRepeatRequest.State>();
        int indexOfNextItem = Array.IndexOf(states, currentRepeatState) + 1;
        return Authentication.SpotifyClient.Player.SetRepeat(new PlayerSetRepeatRequest(states[indexOfNextItem])).Result;
    }
    
    public static bool TogglePLaybackShuffle()
    {
        var currentlyPlayingContext = Authentication.SpotifyClient.Player.GetCurrentPlayback().Result;
        return Authentication.SpotifyClient.Player.SetShuffle(currentlyPlayingContext.ShuffleState ? new PlayerShuffleRequest(false) : new PlayerShuffleRequest(true)).Result;
    }

    public static bool SkipToNextSong()
    {
        return Authentication.SpotifyClient.Player.SkipNext().Result;
    }

    public static bool SkipToPreviousSong()
    {
        return Authentication.SpotifyClient.Player.SkipPrevious().Result;
    }
}
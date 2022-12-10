using SpotifyAPI.Web;

namespace SpotifyAPI;

public static class Player
{
    public static CurrentlyPlaying GetCurrentSong()
    {
        // probably fails if triggered shortly after song changes
        return Authentication.SpotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result;
    }
    
    public static bool SetCurrentPlayingSong(string songId, string playlistId)
    {
        var playlist = PlayList.GetPlaylistById(playlistId);
        var songToPlay = Track.GetTrackById(songId);

        return Authentication.SpotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            ContextUri = playlist.Uri,
            OffsetParam = new PlayerResumePlaybackRequest.Offset
            {
                Uri = songToPlay.Uri
            }
        }).Result;
    }
    
    public static bool SetCurrentPlayingToPlaylist(string playlistId)
    {
        var playlist = PlayList.GetPlaylistById(playlistId);

        return Authentication.SpotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            ContextUri = playlist.Uri 
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
        if (!Enum.TryParse(currentlyPlayingContext.RepeatState, true, out PlayerSetRepeatRequest.State currentRepeatState)) return false;
        var states = Enum.GetValues<PlayerSetRepeatRequest.State>().Reverse().ToArray();
        int indexOfNextItem = Array.IndexOf(states, currentRepeatState) + 1;
        return Authentication.SpotifyClient.Player.SetRepeat(new PlayerSetRepeatRequest(states[indexOfNextItem % states.Length])).Result;
    }
    
    public static bool TogglePlaybackShuffle()
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
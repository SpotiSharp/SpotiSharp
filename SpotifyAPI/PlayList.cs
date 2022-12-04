using SpotifyAPI.Web;

namespace SpotifyAPI;

public class PlayList
{
    public Paging<SimplePlaylist> PlayLists { get; private set; }

    public PlayList()
    {
        if (Authentication.SpotifyClient == null) return;
        PlayLists = GetAllPlaylists();
    }

    public Paging<SimplePlaylist> GetAllPlaylists()
    {
        return Authentication.SpotifyClient.Playlists.CurrentUsers().Result;
    }

    public Task<FullTrack> GetTrackById(string trackId)
    {
        return Authentication.SpotifyClient.Tracks.Get(trackId);
    }

    public Task<FullPlaylist> GetTracksByPlaylistId(string playlistId)
    {
        return Authentication.SpotifyClient.Playlists.Get(playlistId);
    }
}
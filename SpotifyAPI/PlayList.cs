using SpotifyAPI.Web;

namespace SpotifyAPI;

public static class PlayList
{
    public static FullPlaylist GetPlaylistById(string playlistId)
    {
        return Authentication.SpotifyClient.Playlists.Get(playlistId).Result;
    }

    public static Paging<SimplePlaylist> GetAllUserPlaylists()
    {
        return Authentication.SpotifyClient.Playlists.CurrentUsers().Result;
    }

    public static Paging<PlaylistTrack<IPlayableItem>> GetTracksByPlaylistId(string playlistId)
    {
        FullPlaylist playlist = GetPlaylistById(playlistId);
        return playlist.Tracks ??= new Paging<PlaylistTrack<IPlayableItem>>();
    }

    public static List<string> GetTrackIdsByPlaylistId(string playlistId)
    {
        Paging<PlaylistTrack<IPlayableItem>> songs = GetTracksByPlaylistId(playlistId);
        return (from song in songs.Items 
            let songAsTrack = song.Track as FullTrack 
            select songAsTrack.Uri).ToList();
    }
}
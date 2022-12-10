using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class PlaylistListModel
{
    public List<Playlist> PlayLists { get; private set; } = new List<Playlist>();
    
    public PlaylistListModel()
    {
        Paging<SimplePlaylist> userPlaylists;
        try
        {
            userPlaylists = SpotifyAPI.PlayList.GetAllUserPlaylists();
        }
        catch (UnauthorizedAccessException) { return; }
        
        if (userPlaylists.Items == null) return;
        foreach (var playlist in userPlaylists.Items)
        {
            PlayLists.Add(new Playlist(playlist.Id, playlist.Images.ElementAtOrDefault(0)?.Url ?? string.Empty, playlist.Name, playlist.Tracks.Total));
        }
    }
}
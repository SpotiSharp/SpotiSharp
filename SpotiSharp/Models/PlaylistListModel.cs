using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class PlaylistListModel
{
    public List<Playlist> PlayLists { get; private set; } = new List<Playlist>();

    public PlaylistListModel()
    {
        LoadPlaylist();
    }
    
    internal void LoadPlaylist()
    {
        var userPlaylists = SpotifyAPI.APICaller.Instance?.GetAllUserPlaylists();
        var tmpPlaylist = new List<Playlist>();
        if (userPlaylists?.Items == null) return;
        foreach (var playlist in userPlaylists.Items)
        {
            tmpPlaylist.Add(new Playlist(playlist.Id, playlist.Images.ElementAtOrDefault(0)?.Url ?? string.Empty, playlist.Name, playlist.Tracks.Total));
        }

        PlayLists = tmpPlaylist;
    }
}
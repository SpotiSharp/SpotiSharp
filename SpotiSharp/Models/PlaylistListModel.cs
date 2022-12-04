using SpotifyAPI;
using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class PlaylistListModel
{
    public List<Playlist> PlayLists { get; private set; } = new List<Playlist>();
    
    public PlaylistListModel()
    {
        var playlistApi = new PlayList();
        if (playlistApi?.PlayLists?.Items == null) return;
        foreach (var playlist in playlistApi.PlayLists.Items)
        {
            PlayLists.Add(new Playlist(playlist.Images[0].Url, playlist.Name, playlist.Tracks.Total));
        }
    }
}
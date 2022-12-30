namespace SpotiSharp.Models;

public class PlaylistListModel
{
    private static List<Playlist> _playLists = new List<Playlist>();
    
    public static List<Playlist> PlayLists
    {
        get
        {
            LoadPlaylist();
            return _playLists;
        }
        private set => _playLists = value;
    }

    public PlaylistListModel()
    {
        LoadPlaylist();
    }
    
    internal static void LoadPlaylist()
    {
        var userPlaylists = SpotifyAPI.APICaller.Instance?.GetAllUserPlaylists();
        var tmpPlaylist = new List<Playlist>();
        if (userPlaylists?.Items == null) return;
        foreach (var playlist in userPlaylists.Items)
        {
            tmpPlaylist.Add(new Playlist(playlist.Id, playlist.Images.ElementAtOrDefault(0)?.Url ?? string.Empty, playlist.Name, playlist.Tracks.Total));
        }

        _playLists = tmpPlaylist;
    }
}
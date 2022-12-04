using SpotifyAPI;

namespace SpotiSharp.Models;

public class Playlist
{
    public string PlaylistId { get; private set; }
    public string PlayListImageURL { get; private set; }
    public string PlayListTitle { get; private set; }
    public string SongAmount { get; private set; }
    
    public Playlist(string playlistId, string playListImageUrl, string playListTitle, int? songAmount)
    {
        PlaylistId = playlistId;
        PlayListImageURL = playListImageUrl;
        PlayListTitle = playListTitle;
        SongAmount = $"{songAmount} Songs";
    }
}
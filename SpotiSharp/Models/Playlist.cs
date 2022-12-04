using SpotifyAPI;

namespace SpotiSharp.Models;

public class Playlist
{
    public string PlayListImageURL { get; private set; }
    public string PlayListTitle { get; private set; }
    public string SongAmount { get; private set; }
    
    public Playlist(string playListImageUrl, string playListTitle, int? songAmount)
    {
        PlayListImageURL = playListImageUrl;
        PlayListTitle = playListTitle;
        SongAmount = $"{songAmount} Songs";
    }
}
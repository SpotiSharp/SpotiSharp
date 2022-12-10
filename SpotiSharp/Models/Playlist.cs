using SpotifyAPI;

namespace SpotiSharp.Models;

public class Playlist
{
    public string PlayListId { get; private set; }
    public string PlayListImageURL { get; private set; }
    public string PlayListTitle { get; private set; }
    public string SongAmount { get; private set; }
    
    public Playlist(string playListId, string playListImageUrl, string playListTitle, int? songAmount)
    {
        PlayListId = playListId;
        PlayListImageURL = playListImageUrl;
        PlayListTitle = playListTitle;
        SongAmount = $"{songAmount} Songs";
    }
}
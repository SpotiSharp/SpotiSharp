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

    public override bool Equals(object? obj)
    {
        if (obj is not Playlist playlistComp) return false;
        if (playlistComp.PlayListId.Equals(PlayListId) &&
            playlistComp.PlayListImageURL.Equals(PlayListImageURL) &&
            playlistComp.PlayListTitle.Equals(PlayListTitle) &&
            playlistComp.SongAmount.Equals(SongAmount)) return true;
        return false;
    }
}
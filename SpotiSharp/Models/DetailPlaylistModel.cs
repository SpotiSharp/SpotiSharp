namespace SpotiSharp.Models;

public class DetailPlaylistModel
{
    public string GetPlaylistImageURL(string playlistId)
    {
        var images = SpotifyAPI.PlayList.GetPlaylistById(playlistId).Images;
        if (images != null) return images[0].Url;
        return "couldn't get playlist image.";
    }

    public string GetPlaylistName(string playlistId)
    {
        return SpotifyAPI.PlayList.GetPlaylistById(playlistId).Name;
    }
    
    public string GetPlaylistDescription(string playlistId)
    {
        return SpotifyAPI.PlayList.GetPlaylistById(playlistId).Description;
    }
}
using SpotiSharp.Consts;

namespace SpotiSharp.Models;

public class DetailPlaylistModel
{
    public string GetPlaylistImageURL(string playlistId)
    {
        if (playlistId == Constants.LIKED_PLALIST_ID) return Constants.LIKED_PLALIST_IMAGE_URL;
        var images = SpotifyAPI.APICaller.Instance?.GetPlaylistById(playlistId).Images;
        if (images != null) return images.ElementAtOrDefault(0)?.Url ?? string.Empty;
        return "couldn't get playlist image.";
    }

    public string GetPlaylistName(string playlistId)
    {
        return playlistId == Constants.LIKED_PLALIST_ID ? "Liked Songs" : SpotifyAPI.APICaller.Instance?.GetPlaylistById(playlistId).Name ?? "Couldn't load playlist name.";
    }
    
    public string GetPlaylistDescription(string playlistId)
    {
        return playlistId == Constants.LIKED_PLALIST_ID ? "Your liked songs." : SpotifyAPI.APICaller.Instance?.GetPlaylistById(playlistId).Description ?? "Couldn't load playlist description.";
    }
}
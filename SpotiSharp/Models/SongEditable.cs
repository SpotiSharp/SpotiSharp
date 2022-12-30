using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class SongEditable : Song
{
    public int Index { get; private set; }
    
    public SongEditable(int index, FullTrack fullTrack)
    {
        Index = index;
        SongId = fullTrack.Id;
        SongImageURL = fullTrack.Album.Images[0].Url;
        SongTitle = fullTrack.Name;
        SongArtists = string.Join(", ", fullTrack.Artists);
    }
}
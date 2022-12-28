using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class Song
{
    public string SongId { get; private set; }
    public string SongImageURL { get; private set; }
    public string SongTitle { get; private set; }
    public string SongArtists { get; private set; }
    
    public string PartOfPlayListWithId { get; private set; }
    
    public Song(string songId, string songImageURL, string songTitle, string songArtists, string partOfPlayListWithId)
    {
        SongId = songId;
        SongImageURL = songImageURL;
        SongTitle = songTitle;
        SongArtists = songArtists;
        PartOfPlayListWithId = partOfPlayListWithId;
    }

    public Song(FullTrack fullTrack)
    {
        SongId = fullTrack.Id;
        SongImageURL = fullTrack.Album.Images[0].Url;
        SongTitle = fullTrack.Name;
        SongArtists = string.Join(", ", fullTrack.Artists);
    }
}
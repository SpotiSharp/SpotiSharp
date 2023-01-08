using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class Song
{
    public string SongId { get; set; }
    public string SongImageURL { get; set; }
    public string SongTitle { get; set; }
    public string SongArtists { get; set; }
    public string PartOfPlayListWithId { get; private set; }

    public Song() { }
    
    public Song(string songId, string songImageURL, string songTitle, string songArtists, string partOfPlayListWithId)
    {
        SongId = songId;
        SongImageURL = songImageURL;
        SongTitle = songTitle;
        SongArtists = songArtists;
        PartOfPlayListWithId = partOfPlayListWithId;
    }
}
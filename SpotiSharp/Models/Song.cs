namespace SpotiSharp.Models;

public class Song
{
    public string SongImageURL { get; private set; }
    public string SongTitle { get; private set; }
    public string SongArtists { get; private set; }
    
    public Song(string songImageURL, string songTitle, string songArtists)
    {
        SongImageURL = songImageURL;
        SongTitle = songTitle;
        SongArtists = songArtists;
    }
}
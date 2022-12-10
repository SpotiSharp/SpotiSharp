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
}
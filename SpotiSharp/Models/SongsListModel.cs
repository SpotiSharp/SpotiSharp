using SpotifyAPI.Web;

namespace SpotiSharp.Models;

public class SongsListModel
{
    public List<Song> Songs { get; private set; } = new List<Song>();
    
    public SongsListModel(string playlistId)
    {
        var songs = SpotifyAPI.PlayList.GetTracksByPlaylistId(playlistId);
        if (songs?.Items == null) return;
        foreach (var playableObject in songs.Items.ToArray())
        {
            if (playableObject.Track.Type == ItemType.Track)
            {
                if (playableObject.Track is not FullTrack song) continue;
                Songs.Add(new Song(song.Id, song.Album.Images[0].Url, song.Name, string.Join(", ", song.Artists.Select(x => x.Name)), playlistId));
            }
            else if (playableObject.Track.Type == ItemType.Episode)
            {
                if (playableObject.Track is not FullEpisode episode) continue;
                Songs.Add(new Song(episode.Id, episode.Images[0].Url, episode.Name, "", playlistId));
            }
        }
    }
}
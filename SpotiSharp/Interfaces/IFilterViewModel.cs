using SpotifyAPI.Web;

namespace SpotiSharp.Interfaces;

public interface IFilterViewModel
{
    public List<FullTrack> FilterSongs(List<FullTrack> songs);
}
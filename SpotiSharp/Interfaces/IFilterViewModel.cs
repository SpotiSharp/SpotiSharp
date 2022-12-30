using SpotifyAPI.Web;

namespace SpotiSharp.Interfaces;

public interface IFilterViewModel
{
    public Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures);
}
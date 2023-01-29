using SpotifyAPI.Web;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Interfaces;

public interface IFilterViewModel
{
    public Guid GetGuid();
    
    public TrackFilter GetTrackFilter();

    public void SyncValues(List<object> values);
    
    public Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures);
    
    public void RemoveFilter();
}
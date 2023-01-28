using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Interfaces;

public interface IFilterViewModel
{
    public TrackFilter GetTrackFilter();
    
    public Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures);
    
    public void RemoveFilter();
}
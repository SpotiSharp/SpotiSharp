using SpotifyAPI.Web;
using SpotiSharp.Enums;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels.Filters;

public class PlaylistTextFilterViewModel : BaseViewModel, IFilterViewModel
{
    private TrackFilter _trackFilterName;

    public string FilterName
    {
        get { return _trackFilterName.ToString(); }
        set { SetProperty(ref _trackFilterName, Enum.Parse<TrackFilter>(value)); }
    }

    private string _genreName = String.Empty;
    
    public string GenreName
    {
        get { return _genreName; }
        set { SetProperty(ref _genreName, value); }
    }
    
    public PlaylistTextFilterViewModel(TrackFilter trackFilter)
    {
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
    }

    public async Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures)
    {
        return await _trackFilterName.GetFilterFunction()(fullTracks, audioFeatures, GenreName);
    }
}
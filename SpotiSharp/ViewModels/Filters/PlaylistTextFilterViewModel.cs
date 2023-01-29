using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.ViewModels.Filters;

public class PlaylistTextFilterViewModel : BaseFilter, IFilterViewModel
{
    private Guid _guid = Guid.NewGuid();
    
    private TrackFilter _trackFilter;

    public string FilterName
    {
        get { return _trackFilter.ToString(); }
        set { SetProperty(ref _trackFilter, Enum.Parse<TrackFilter>(value)); }
    }

    public Guid GetGuid()
    {
        return _guid;
    }
    
    public TrackFilter GetTrackFilter()
    {
        return _trackFilter;
    }
    
    private string _genreName = String.Empty;
    
    public string GenreName
    {
        get { return _genreName; }
        set { SetProperty(ref _genreName, value); }
    }
    
    public PlaylistTextFilterViewModel(TrackFilter trackFilter)
    {
        RemoveFilterCommand = new Command(RemoveFilterFromCommand);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
    }
    
    public PlaylistTextFilterViewModel(TrackFilter trackFilter, Guid guid, List<object> parameters)
    {
        RemoveFilterCommand = new Command(RemoveFilterFromCommand);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
        if (guid == Guid.Empty)
        {
            CollaborationAPI.Instance?.SetFiltersOfSession();
            return;
        }
        _guid = guid;
        GenreName = parameters[0].ToString();
    }
    
    public void SyncValues(List<object> values)
    {
        GenreName = values[0].ToString();
    }

    public async Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures)
    {
        return await _trackFilter.GetFilterFunction()(fullTracks, audioFeatures, GenreName);
    }

    private void RemoveFilterFromCommand()
    {
        RemoveFilter();
        if (StorageHandler.IsUsingCollaborationHost) CollaborationAPI.Instance?.SetFiltersOfSession();
    }
    
    public void RemoveFilter()
    {
        int index = PlaylistCreatorPageModel.Filters.IndexOf(this);
        InvokeRemoveEvent(index);
    }
    
    public ICommand RemoveFilterCommand { private set; get; }
}
using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.ViewModels.Filters;

public class PlaylistRangeFilterViewModel : BaseFilter, IFilterViewModel
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
    
    private int _sliderValue = 0;

    public int SliderValue
    {
        get { return _sliderValue; }
        set { SetProperty(ref _sliderValue, value); }
    }
    
    private NumericFilterOption _selectedfilterOption = NumericFilterOption.Equal;

    public NumericFilterOption SelectedFilterOption
    {
        get { return _selectedfilterOption; }
        set { SetProperty(ref _selectedfilterOption, value); }
    }
    
    private List<NumericFilterOption> _filterOptions = Enum.GetValues<NumericFilterOption>().ToList();

    public List<NumericFilterOption> FilterOptions
    {
        get { return _filterOptions; }
        set { SetProperty(ref _filterOptions, value); }
    }

    public PlaylistRangeFilterViewModel(TrackFilter trackFilter)
    {
        RemoveFilterCommand = new Command(RemoveFilterFromCommand);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
    }
    
    public PlaylistRangeFilterViewModel(TrackFilter trackFilter, Guid guid, List<object> parameters)
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
        SelectedFilterOption = Enum.Parse<NumericFilterOption>(parameters[0].ToString());
        SliderValue = Convert.ToInt32(parameters[1]);
    }
    
    public void SyncValues(List<object> values)
    {
        SelectedFilterOption = Enum.Parse<NumericFilterOption>(values[0].ToString());
        SliderValue = Convert.ToInt32(values[1]);
    }
    
    public async Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures)
    {
        return await _trackFilter.GetFilterFunction()(fullTracks, audioFeatures, SliderValue, SelectedFilterOption);
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
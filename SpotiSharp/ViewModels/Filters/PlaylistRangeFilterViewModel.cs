using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.ViewModels.Filters;

public delegate void RangeValueCallback()  ;

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

    private bool _isInCallBack = false;

    private int? _lastPendingSliderValue = null;

    private int? _pendingSliderValue = null;
    
    private int _sliderValue = 0;

    public int SliderValue
    {
        get { return _sliderValue; }
        set 
        {
            _pendingSliderValue = value;
            if (_pendingSliderValue != _lastPendingSliderValue && !_isInCallBack)
            {
                StartRangeValueCallback?.Invoke();
            }
            else
            {
                SetProperty(ref _sliderValue, value);
            }

            _lastPendingSliderValue = _pendingSliderValue;
        }
    }
    
    private async void ProcessRangeValueCallback()
    {
        _isInCallBack = true;
        await Task.Delay(200);
        _isInCallBack = false;
        SliderValue = _pendingSliderValue ?? 0;
        CollaborationAPI.Instance?.SetFiltersOfSession();
    }
    
    public event RangeValueCallback StartRangeValueCallback;
    
    public int SliderValueUiRefresh
    {
        get { return _sliderValue; }
        set { SetProperty(ref _sliderValue, value, propertyName: "SliderValue"); }
    }
    
    private NumericFilterOption _selectedFilterOption = NumericFilterOption.Equal;

    public NumericFilterOption SelectedFilterOption
    {
        get { return _selectedFilterOption; }
        set
        {
            SetProperty(ref _selectedFilterOption, value);
            CollaborationAPI.Instance?.SetFiltersOfSession();
        }
    }
    
    public NumericFilterOption SelectedFilterOptionUiRefresh
    {
        get { return _selectedFilterOption; }
        set { SetProperty(ref _selectedFilterOption, value, propertyName: "SelectedFilterOption"); }
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
        StartRangeValueCallback += ProcessRangeValueCallback;
    }

    public PlaylistRangeFilterViewModel(TrackFilter trackFilter, Guid guid, List<object> parameters)
    {
        RemoveFilterCommand = new Command(RemoveFilterFromCommand);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
        StartRangeValueCallback += ProcessRangeValueCallback;
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
        SelectedFilterOptionUiRefresh = Enum.Parse<NumericFilterOption>(values[0].ToString());
        SliderValueUiRefresh = Convert.ToInt32(values[1]);
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
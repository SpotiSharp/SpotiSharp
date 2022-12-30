using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Enums;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels.Filters;

public class PlaylistNumberFilterViewModel : BaseFilter, IFilterViewModel
{
    private TrackFilter _trackFilterName;

    public string FilterName
    {
        get { return _trackFilterName.ToString(); }
        set { SetProperty(ref _trackFilterName, Enum.Parse<TrackFilter>(value)); }
    }
    
    private string _enteredNumber = string.Empty;

    public string EnteredNumber
    {
        get { return _enteredNumber; }
        set { SetProperty(ref _enteredNumber, value); }
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

    public PlaylistNumberFilterViewModel(TrackFilter trackFilter)
    {
        RemoveFilterCommand = new Command(RemoveFilter);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
    }
    
    public async Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures)
    {
        return await _trackFilterName.GetFilterFunction()(fullTracks, audioFeatures, EnteredNumber, SelectedFilterOption);
    }

    public void RemoveFilter()
    {
        int index = PlaylistCreatorPageModel.Filters.IndexOf(this);
        InvokeEvent(index);
    }
    
    public ICommand RemoveFilterCommand { private set; get; }
}
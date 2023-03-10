using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharp.Models;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.ViewModels.Filters;

public class PlaylistNumberFilterViewModel : BaseFilter, IFilterViewModel
{
    private Guid _guid = Guid.NewGuid();
    
    private List<char> _allowedChars = new List<char>
    {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9'
    };
    
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

    private string _enteredNumber = string.Empty;

    public string EnteredNumber
    {
        get { return _enteredNumber; }
        set
        {
            OnlyNumerics(value);
            SetProperty(ref _enteredNumber, value);
            CollaborationAPI.Instance?.SetFiltersOfSession();
        }
    }
    
    public string EnteredNumberUiRefresh
    {
        get { return _enteredNumber; }
        set
        {
            OnlyNumerics(value);
            SetProperty(ref _enteredNumber, value, propertyName: "EnteredNumber");
        }
    }

    private string _validationMessage = string.Empty;
    
    public string ValidationMessage
    {
        get { return _validationMessage; }
        set { SetProperty(ref _validationMessage, value); }
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

    public PlaylistNumberFilterViewModel(TrackFilter trackFilter)
    {
        RemoveFilterCommand = new Command(RemoveFilterFromCommand);
        PlaylistCreatorPageModel.Filters.Add(this);
        FilterName = trackFilter.ToString();
    }
    
    public PlaylistNumberFilterViewModel(TrackFilter trackFilter, Guid guid, List<object> parameters)
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
        EnteredNumber = parameters[1].ToString();
    }

    public void SyncValues(List<object> values)
    {
        SelectedFilterOptionUiRefresh = Enum.Parse<NumericFilterOption>(values[0].ToString());
        EnteredNumberUiRefresh = values[1].ToString();
    }

    private void OnlyNumerics(string input)
    {
        string result = string.Empty;
        List<char> characters = input.ToList();
        foreach (var character in characters)
        {
            if (_allowedChars.Contains(character)) result += character;
        }

        if (!input.Equals(result))
        {
            ValidationMessage = "Value entered in number field must be a number.\nFilter will be ignored.";
        }
        else
        {
            ValidationMessage = string.Empty;
        }
    }
     
    public async Task<List<FullTrack>> FilterSongs(List<FullTrack> fullTracks, List<TrackAudioFeatures> audioFeatures)
    {
        if (ValidationMessage == string.Empty && EnteredNumber != string.Empty) return await _trackFilter.GetFilterFunction()(fullTracks, audioFeatures, int.Parse(EnteredNumber), SelectedFilterOption);
        return fullTracks;
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
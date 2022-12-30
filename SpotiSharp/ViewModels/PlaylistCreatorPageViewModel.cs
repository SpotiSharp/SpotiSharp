using System.Windows.Input;
using SpotiSharp.Enums;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public delegate void AddingFilter(TrackFilter trackFilter);


public class PlaylistCreatorPageViewModel : BaseViewModel
{
    public static event AddingFilter OnAddingFilter;
    
    private string _selectedPlaylistNameId;

    public string SelectedPlaylistNameId
    {
        get { return _selectedPlaylistNameId; }
        set { SetProperty(ref _selectedPlaylistNameId, value); }
    }

    private List<string> _playlistNamesIds;

    public List<string> PlaylistNamesIds
    {
        get { return _playlistNamesIds; }
        set { SetProperty(ref _playlistNamesIds, value); }
    }
    
    private TrackFilter _selectedFilter;

    public string SelectedFilter
    {
        get { return _selectedFilter.ToString(); }
        set { SetProperty(ref _selectedFilter, Enum.Parse<TrackFilter>(value)); }
    }

    private List<TrackFilter> _filters = Enum.GetValues<TrackFilter>().ToList();

    public List<string> Filters
    {
        get { return _filters.Select(f => f.ToString()).ToList(); }
        set { SetProperty(ref _filters, value.Select(Enum.Parse<TrackFilter>).ToList() ); }
    }

    private bool _isFilteringPlaylist = false;
    
    public bool IsFilteringPlaylist
    {
        get { return _isFilteringPlaylist; }
        set { SetProperty(ref _isFilteringPlaylist, value ); }
    }

    public PlaylistCreatorPageViewModel()
    {
        AddSongsFromPlaylist = new Command(AddSongsFromPlaylistHandler);
        AddFilter = new Command(AddFilterHandler);
        ApplyFilters = new Command(ApplyFiltersHandler);
        PlaylistCreationSonglistViewModel.OnPlalistIsFiltered += () => IsFilteringPlaylist = false;
    }

    internal override void OnAppearing()
    {
        PlaylistNamesIds = PlaylistListModel.PlayLists.Select(p => $"{p.PlayListTitle}\n{p.PlayListId}").ToList();
    }

    private void AddSongsFromPlaylistHandler()
    {
        if (SelectedPlaylistNameId != null) PlaylistCreatorPageModel.AddSongsFromPlaylist(SelectedPlaylistNameId.Split("\n")[1]);
    }

    private void AddFilterHandler()
    {
        OnAddingFilter?.Invoke(_selectedFilter); 
    }

    private void ApplyFiltersHandler()
    {
        IsFilteringPlaylist = true;
        PlaylistCreatorPageModel.ApplyFilters();
    }

    public ICommand AddSongsFromPlaylist { private set; get; }
    public ICommand AddFilter { private set; get; }
    public ICommand ApplyFilters { private set; get; }

}
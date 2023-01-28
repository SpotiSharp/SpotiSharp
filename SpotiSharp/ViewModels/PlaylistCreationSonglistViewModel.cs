using System.Collections.ObjectModel;
using System.Windows.Input;
using SpotifyAPI.Web;
using SpotiSharp.Models;
using SpotiSharpBackend;

namespace SpotiSharp.ViewModels;

public delegate void PlaylistIsFiltered();

public class PlaylistCreationSonglistViewModel : BaseViewModel
{
    public static event PlaylistIsFiltered OnPlalistIsFiltered;

    private string _filteredAndNonFilterSongCount = "0 / 0 Songs";
    
    public string FilteredAndNonFilterSongCount
    {
        get { return _filteredAndNonFilterSongCount; }
        set { SetProperty(ref _filteredAndNonFilterSongCount, value); }
    } 
    
    private ObservableCollection<object> _selectedItems = new ObservableCollection<object>();
    
    public ObservableCollection<object> SelectedItems
    {
        get { return _selectedItems; }
        set { SetProperty(ref _selectedItems, value); }
    } 
    
    
    private List<SongEditable> _songs = new List<SongEditable>();

    public List<SongEditable> Songs
    {
        get { return _songs; }
        set { SetProperty(ref _songs, value); }
    }

    public PlaylistCreationSonglistViewModel()
    {
        RemoveSongs = new Command(RemoveSongsHandler);
        ClearSongs = new Command(() => PlaylistCreatorPageModel.UnfilteredSongs = new List<FullTrack>());
        PlaylistCreatorPageModel.OnSongsUiUpdate += RefreshSongs;
    }

    public static void PlaylistFinishedFiltering()
    {
        OnPlalistIsFiltered?.Invoke();
    }

    private async void RefreshSongs()
    {

        List<FullTrack> fullTracksFiltered;
        if (StorageHandler.IsUsingCollaborationHost)
        {
            fullTracksFiltered = PlaylistCreatorPageModel.CurrentFilteredSongs;
        }
        else
        {
            fullTracksFiltered = await PlaylistCreatorPageModel.GetFilteredSongs();
            PlaylistCreatorPageModel.CurrentFilteredSongs = fullTracksFiltered;
        }
        
        
        Songs = fullTracksFiltered.Select((fullTrack, index) =>
        {
            return new SongEditable(index, fullTrack);
        }).ToList();
        FilteredAndNonFilterSongCount = $"{Songs.Count} / {PlaylistCreatorPageModel.UnfilteredSongs.Count} Songs";
        PlaylistFinishedFiltering();
    }
    
    private void RemoveSongsHandler()
    {
        PlaylistCreatorPageModel.RemoveSongsByIndex(SelectedItems.Select(si =>
        {
            if (si is SongEditable songEditable) return songEditable.Index;
            return 0;
        }).ToList());
    }

    public ICommand RemoveSongs { private set; get; }
    public ICommand ClearSongs { private set; get; }
}
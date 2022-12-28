using System.Windows.Input;
using SpotifyAPI;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class PlaylistCreatorPageViewModel : BaseViewModel
{
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

    public PlaylistCreatorPageViewModel()
    {
        AddSongsFromPlaylist = new Command(AddSongsFromPlaylistHandler);
        AddFilter = new Command(AddFilterHandler);

        var playlistListModel = new PlaylistListModel();
        // TODO: what if playlist load fails first time (may need to implement some sort of retry)
        PlaylistNamesIds = playlistListModel.PlayLists.Select(p => $"{p.PlayListTitle}\n{p.PlayListId}").ToList();
    }
    
    private void AddSongsFromPlaylistHandler()
    {
        if (SelectedPlaylistNameId != null) PlaylistCreatorPageModel.AddSongsFromPlaylist(SelectedPlaylistNameId.Split("\n")[1]);
    }
    
    private void AddFilterHandler()
    {
        throw new NotImplementedException();
    }
    
    public ICommand AddSongsFromPlaylist { private set; get; }
    public ICommand AddFilter { private set; get; }

}
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class PlaylistListViewModel : BaseViewModel
{
    private const int REFRESH_DELAY_IN_UILOOP_INTERVALS = 10;
    
    private int _updateDelayCounter = 0;

    private PlaylistListModel _playlistListModel;

    private Playlist _selectedPlaylist;

    public Playlist SelectedPlaylist
    {
        get { return _selectedPlaylist; }
        set { SetProperty(ref _selectedPlaylist, value); }
    }

    private List<Playlist> _playLists;

    public List<Playlist> PlayLists
    {
        get { return _playLists; }
        set { SetProperty(ref _playLists, value); }
    }

    public PlaylistListViewModel()
    {
        _playlistListModel = new PlaylistListModel();
        PlayLists = _playlistListModel.PlayLists;
        UiLoop.Instance.OnRefreshUi += RefreshPlaylist;
    }

    private void RefreshPlaylist()
    {
        if (REFRESH_DELAY_IN_UILOOP_INTERVALS == _updateDelayCounter)
        {
            _playlistListModel.LoadPlaylist();
            if (!comparePlaylists(PlayLists, _playlistListModel.PlayLists)) PlayLists = _playlistListModel.PlayLists;
            _updateDelayCounter = 0;
        }
        else
        {
            _updateDelayCounter++;
        }
    }

    private bool comparePlaylists(List<Playlist> playlists1, List<Playlist> playlists2)
    {
        if (playlists1.Count != playlists2.Count) return false;
        for (int i = 0; i < playlists1.Count; i++)
        {
            if (!playlists1[i].Equals(playlists2[i])) return false;
        }

        return true;
    }

    public async void GoToPlaylistDetail()
    {
        if (SelectedPlaylist == null) return;
        string playlistId = SelectedPlaylist.PlayListId;
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "PlaylistId",  playlistId}
        };

        await Shell.Current.GoToAsync($"DetailPlaylistPage", navigationParameter);
    }
}
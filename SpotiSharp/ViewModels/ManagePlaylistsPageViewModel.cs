namespace SpotiSharp.ViewModels;

public class ManagePlaylistsPageViewModel : BaseViewModel
{
    private static ManagePlaylistsPageViewModel _managePlaylistsPageViewModel = null;
    public static ManagePlaylistsPageViewModel Instance
    {
        get
        {
            if (_managePlaylistsPageViewModel == null) _managePlaylistsPageViewModel = new ManagePlaylistsPageViewModel();
            return _managePlaylistsPageViewModel;
        }
    }

    private ManagePlaylistsPageViewModel() {}
}
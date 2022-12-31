using SpotifyAPI;

namespace SpotiSharp.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    private bool _isUserIsNotAuthenticated = Authentication.SpotifyClient == null;

    public bool IsUserIsNotAuthenticated
    {
        get { return _isUserIsNotAuthenticated; }
        set { SetProperty(ref _isUserIsNotAuthenticated, value); }
    }

    internal override void OnAppearing()
    {
        IsUserIsNotAuthenticated = Authentication.SpotifyClient == null;
    }
}
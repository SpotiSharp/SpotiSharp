using System.Windows.Input;
using SpotiSharpBackend;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class AuthenticationPageViewModel : BaseViewModel
{
    private string? _profilePictureURL;
    
    public string? ProfilePictureURL
    {
        get { return _profilePictureURL; }
        set { SetProperty(ref _profilePictureURL, value); }
    }
    
    private Color _authenticationStatusColor;
    
    public Color AuthenticationStatusColor
    {
        get { return _authenticationStatusColor; }
        set { SetProperty(ref _authenticationStatusColor, value); }
    }
    
    private string _userName;
    
    public string UserName
    {
        get { return _userName; }
        set { SetProperty(ref _userName, value); }
    }
    
    private string _clientId;
    
    public string ClientId
    {
        get { return _clientId; }
        set { SetProperty(ref _clientId, value); }
    }
    
    public AuthenticationPageViewModel()
    {
        ConnectToSpotifyAPI = new Command(() => { if (ClientId != null && ClientId != string.Empty) Authentication.Authenticate(ClientId); });
        OpenSpotifyDevDashBoard = new Command(() => Browser.Default.OpenAsync("https://developer.spotify.com/dashboard/", BrowserLaunchMode.SystemPreferred));
        Authentication.OnAuthenticate += RefreshProfile;
        ClientId = Task.Run(async () => await SecureStorage.Default.GetAsync("clientId")).Result;
    }

    internal override void OnAppearing()
    {
        RefreshProfile();
    }

    private void RefreshProfile()
    {
        var profile = new Profile();
        UserName = profile.UserName ?? "Not Authenticated";
        ProfilePictureURL = profile.ProfilePictureURL;
        AuthenticationStatusColor = profile.IsAuthenticated ? Brush.Green.Color : Brush.Red.Color;

    }
    
    public ICommand ConnectToSpotifyAPI { private set; get; }
    
    public ICommand OpenSpotifyDevDashBoard { private set; get; }

}
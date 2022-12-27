using System.Windows.Input;
using SpotifyAPI;
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
    
    private string _clientId = SecureStorage.Default.GetAsync("clientId").Result;
    
    public string ClientId
    {
        get { return _clientId; }
        set { SetProperty(ref _clientId, value); }
    }
    
    public AuthenticationPageViewModel()
    {
        ConnectToSpotifyAPI = new Command(ConnectToSpotifyAPIFunc);
        Authentication.OnAuthenticate += RefreshProfile;
    }

    private void ConnectToSpotifyAPIFunc()
    {
        Authentication.Authenticate(ClientId);
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

}
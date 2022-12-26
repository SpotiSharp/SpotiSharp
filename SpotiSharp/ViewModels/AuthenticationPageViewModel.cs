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
    
    private Brush _authenticationStatusColor;
    
    public Brush AuthenticationStatusColor
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
    
    public AuthenticationPageViewModel()
    {
        ConnectToSpotifyAPI = new Command(ConnectToSpotifyAPIFunc);
    }

    private void ConnectToSpotifyAPIFunc()
    {
        Authentication.Authenticate();
    }

    private void RefreshProfile()
    {
        var profile = new Profile();
        UserName = profile.UserName ?? "Not Authenticated";
        ProfilePictureURL = profile.ProfilePictureURL;
        AuthenticationStatusColor = profile.IsAuthenticated ? Brush.Green : Brush.Red;

    }
    
    public ICommand ConnectToSpotifyAPI { private set; get; }

}
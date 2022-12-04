using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpotifyAPI;

namespace SpotiSharp.ViewModels;

public class AuthenticationPageViewModel : INotifyPropertyChanged
{
    private string _profilePictureURL;
    
    public string ProfilePictureURL
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
    
    public event PropertyChangedEventHandler PropertyChanged;

    public AuthenticationPageViewModel()
    {
        ConnectToSpotifyAPI = new Command(ConnectToSpotifyAPIFunc);
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    private void ConnectToSpotifyAPIFunc()
    {
        Authentication.Authenticate();
    }

    private void RefreshProfile()
    {
        var profile = new Profile();
        UserName = profile.UserName != null ? profile.UserName : "Not Authenticated";
        ProfilePictureURL = profile.ProfilePictureURL != null ? profile.UserName : string.Empty;
        AuthenticationStatusColor = profile.IsAuthenticated ? Brush.Green : Brush.Red;

    }
    
    public ICommand ConnectToSpotifyAPI { private set; get; }

}
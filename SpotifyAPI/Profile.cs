using SpotifyAPI.Web;

namespace SpotifyAPI;

public class Profile
{
    public string UserName { get; private set; }
    public string ProfilePictureURL { get; private set; }
    
    public Followers UserFollows { get; private set; }

    public bool IsAuthenticated { get; private set; } = false;

    public Profile()
    {
        if (Authentication.SpotifyClient == null) return;
        UserName = GetUserName();
        ProfilePictureURL = GetProfilePictureURL();
        UserFollows = GetUserFollows();
        IsAuthenticated = true;
    }
    
    public string GetUserName()
    {
        return Authentication.SpotifyClient.UserProfile.Current().Result.DisplayName;
    }
    
    public string GetProfilePictureURL()
    {
        return Authentication.SpotifyClient.UserProfile.Current().Result.Images.ElementAtOrDefault(0)?.Url ?? string.Empty;
    }

    public Followers GetUserFollows()
    {
        return Authentication.SpotifyClient.UserProfile.Current().Result.Followers;
    }
}
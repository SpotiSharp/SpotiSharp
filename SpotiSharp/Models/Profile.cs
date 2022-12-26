using SpotifyAPI;
using SpotifyAPI.Web;


namespace SpotiSharp.Models;

public class Profile
{
    public string? UserName { get; private set; }
    public string? ProfilePictureURL { get; private set; }
    public Followers? UserFollows { get; private set; }
    public bool IsAuthenticated { get; private set; } = false;

    public Profile()
    {
        UserName = APICaller.Instance?.GetUserName();
        ProfilePictureURL = APICaller.Instance?.GetProfilePictureURL();
        UserFollows = APICaller.Instance?.GetUserFollows();
        IsAuthenticated = Authentication.SpotifyClient != null;
    }
}
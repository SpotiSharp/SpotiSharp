using Microsoft.Maui.ApplicationModel;
using SpotifyAPI.Web;

namespace SpotifyAPI;

public static class Authentication
{
    private static string _verifier;
    private static PKCETokenResponse _initialResponse;
    // loaded from local file for development
    private static readonly string _clientId = File.ReadAllLines("./SpotifyKeys/clientId.txt")[0];

    private static SpotifyClient _spotifyClient;
    public static SpotifyClient SpotifyClient 
    {
        get
        {
            if (_spotifyClient == null) throw new UnauthorizedAccessException();
            return _spotifyClient;
        }
        private set { _spotifyClient = value; }
    }

    public static void Authenticate()
    {
        if (_spotifyClient == null)
        {
            NewAuthentication();
        }
        else
        {
            RefreshAuthentication();
        }
    }
    
    private static async void NewAuthentication()
    {
        // Generates a secure random verifier of length 100 and its challenge
        (_verifier,string challenge) = PKCEUtil.GenerateCodes();
        
        var loginRequest = new LoginRequest(
            new Uri("http://127.0.0.1:5000/callback"),
            _clientId,
            LoginRequest.ResponseType.Code
        )
        {
            CodeChallengeMethod = "S256",
            CodeChallenge = challenge,
            Scope = new[] {
                Scopes.Streaming, 
                Scopes.AppRemoteControl, 
                Scopes.PlaylistModifyPrivate, 
                Scopes.PlaylistModifyPublic, 
                Scopes.PlaylistReadCollaborative, 
                Scopes.PlaylistReadPrivate, 
                Scopes.UgcImageUpload, 
                Scopes.UserFollowModify, 
                Scopes.UserFollowRead, 
                Scopes.UserLibraryModify, 
                Scopes.UserLibraryRead, 
                Scopes.UserReadEmail, 
                Scopes.UserReadPrivate, 
                Scopes.UserTopRead, 
                Scopes.UserModifyPlaybackState, 
                Scopes.UserReadCurrentlyPlaying, 
                Scopes.UserReadPlaybackPosition, 
                Scopes.UserReadPlaybackState, 
                Scopes.UserReadRecentlyPlayed
            }
        };
        // start webserver for callback
        var callBackListener = new CallBackListener();
        callBackListener.StartListener();
        
        var uri = loginRequest.ToUri();
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private static async void RefreshAuthentication()
    {
        var newResponse = await new OAuthClient().RequestToken(
            new PKCETokenRefreshRequest(_clientId, _initialResponse.RefreshToken)
        );

        SpotifyClient = new SpotifyClient(newResponse.AccessToken);
    }

    internal static async Task GetCallback(string code)
    {
        _initialResponse = await new OAuthClient().RequestToken(
            new PKCETokenRequest(_clientId, code, new Uri("http://127.0.0.1:5000/callback"), _verifier)
        );

        SpotifyClient = new SpotifyClient(_initialResponse.AccessToken);
    }
}
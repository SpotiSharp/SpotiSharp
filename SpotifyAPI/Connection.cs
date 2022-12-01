using SpotifyAPI.Web;

namespace SpotifyAPI;

public class Connection
{
    public SpotifyClient SpotifyClient { get; }
    
    public Connection()
    {
        // loading api keys from files for development
        string _clientId = File.ReadAllLines("./SpotifyKeys/clientId.txt")[0];
        string _clientSecret = File.ReadAllLines("./SpotifyKeys/clientSecret.txt")[0];
        
        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(_clientId, _clientSecret));
        
        SpotifyClient = new SpotifyClient(config);
    }
}
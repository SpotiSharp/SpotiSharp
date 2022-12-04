using SpotifyAPI.Web;

namespace SpotifyAPI;

public static class Track
{

    public static FullTrack GetTrackById(string trackId)
    {
        return Authentication.SpotifyClient.Tracks.Get(trackId).Result;
    }
    
    public static TracksResponse GetMultipleTrackByTrackId(List<string> trackIds)
    {
        return Authentication.SpotifyClient.Tracks.GetSeveral(new TracksRequest(trackIds)).Result;
    }

    public static TrackAudioFeatures GetAudioFeaturesByTrackId(string trackId)
    {
        return Authentication.SpotifyClient.Tracks.GetAudioFeatures(trackId).Result;
    }

    public static TracksAudioFeaturesResponse GetMultipleAudioFeaturesByTrackIds(List<string> trackIds)
    {
        return Authentication.SpotifyClient.Tracks.GetSeveralAudioFeatures(new TracksAudioFeaturesRequest(trackIds)).Result;
    }
}
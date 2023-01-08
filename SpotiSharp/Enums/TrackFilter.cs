using SpotiSharpBackend;
using SpotifyAPI.Web;

namespace SpotiSharp.Enums;

public enum TrackFilter
{
    Genre,
    Popularity,
    Danceability,
    Energy,
    Positivity,
    Tempo
    
}

public delegate TResult ParamsFunc<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, params T3[] arg3);

public static class FilterMethods
{
    private static Func<int, int, NumericFilterOption, bool> filterByNumericOption = (valueFromSpotify, valueEnteredByUser, option) =>
    {
        switch (option)
        {
            case NumericFilterOption.Equal:
                return valueFromSpotify == valueEnteredByUser;
            case NumericFilterOption.MoreThan:
                return valueFromSpotify >= valueEnteredByUser;
            case NumericFilterOption.LessThan:
                return valueFromSpotify <= valueEnteredByUser;
            default:
                return false;
        }
    };
    
    public static ParamsFunc<List<FullTrack>, List<TrackAudioFeatures>, object, Task<List<FullTrack>>> GetFilterFunction(this TrackFilter trackFilter)
    {
        switch (trackFilter)
        {
            case TrackFilter.Genre:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    string enteredGenre = paramsIn[0] as string ?? string.Empty;

                    var filteredTracks = new List<FullTrack>();
                    foreach (var fullTrack in fullTracks)
                    {
                        var generesOfSong = new List<string>();
                        foreach (var artist in fullTrack.Artists)
                        {
                            var apiCallerInstance = await APICaller.WaitForRateLimitWindowInstance;
                            generesOfSong.AddRange(apiCallerInstance?.GetGenresByArtistId(artist.Id).Select(genreName => genreName.ToLower()) ?? new List<string>());
                        }

                        var totalGeneres = new List<string>();
                        foreach (var genereOfSong in generesOfSong)
                        {
                            totalGeneres.Add(genereOfSong);
                            string[] splitGenere = genereOfSong.Split(" ");
                            if (splitGenere.Length > 0) totalGeneres.AddRange(splitGenere);
                        }
                        
                        if (totalGeneres.Contains(enteredGenre.ToLower()))
                        {
                            filteredTracks.Add(fullTrack);
                        }
                    }

                    return filteredTracks;
                };
            case TrackFilter.Popularity:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    int enteredPopularity = paramsIn[0] as int? ?? 0;
                    NumericFilterOption numericFilterOption = paramsIn[1] as NumericFilterOption? ?? NumericFilterOption.Equal;

                    return fullTracks.Where(fullTrack => filterByNumericOption(fullTrack.Popularity, enteredPopularity, numericFilterOption)).ToList();
                };
            case TrackFilter.Danceability:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    int enteredDanceability = paramsIn[0] as int? ?? 0;
                    NumericFilterOption numericFilterOption = paramsIn[1] as NumericFilterOption? ?? NumericFilterOption.Equal;

                    return (from pair in fullTracks.Select((value, index) => new { value, index })
                        where filterByNumericOption((int)(tracksAudioFeatures[pair.index].Danceability * 100), enteredDanceability, numericFilterOption)
                        select pair.value).ToList();
                    
                };
            case TrackFilter.Energy:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    int enteredEnergy = paramsIn[0] as int? ?? 0;
                    NumericFilterOption numericFilterOption = paramsIn[1] as NumericFilterOption? ?? NumericFilterOption.Equal;

                    return (from pair in fullTracks.Select((value, index) => new { value, index })
                        where filterByNumericOption((int)(tracksAudioFeatures[pair.index].Energy * 100), enteredEnergy, numericFilterOption)
                        select pair.value).ToList();
                };
            case TrackFilter.Positivity:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    int enteredPositivity = paramsIn[0] as int? ?? 0;
                    NumericFilterOption numericFilterOption = paramsIn[1] as NumericFilterOption? ?? NumericFilterOption.Equal;

                    return (from pair in fullTracks.Select((value, index) => new { value, index })
                        where filterByNumericOption((int)(tracksAudioFeatures[pair.index].Valence * 100), enteredPositivity, numericFilterOption)
                        select pair.value).ToList();
                };
            case TrackFilter.Tempo:
                return async (fullTracks, tracksAudioFeatures, paramsIn) =>
                {
                    int enteredTempo = paramsIn[0] as int? ?? 0;
                    NumericFilterOption numericFilterOption = paramsIn[1] as NumericFilterOption? ?? NumericFilterOption.Equal;

                    return (from pair in fullTracks.Select((value, index) => new { value, index })
                        where filterByNumericOption((int)tracksAudioFeatures[pair.index].Tempo, enteredTempo, numericFilterOption)
                        select pair.value).ToList();
                };
            default:
                return null;
        }
    }
}
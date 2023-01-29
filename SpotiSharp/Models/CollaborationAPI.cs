using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotiSharp.Interfaces;
using SpotiSharp.ViewModels;
using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Models;

public class CollaborationAPI
{
    private static CollaborationAPI? _collaborationAPI;
    public static CollaborationAPI? Instance
    {
        get
        {
            if (!StorageHandler.IsUsingCollaborationHost) return null;
            return _collaborationAPI ??= new CollaborationAPI();
        }
    }

    private HttpClient _client = new HttpClient();

    private CollaborationAPI()
    {
        _client.DefaultRequestHeaders.Add("User-Agent", "SpotiSharpMauiApp");
        _client.DefaultRequestHeaders.Add("Accept", "*/*");
    }

    public async Task GetSession(string sessionId)
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get?sessionId={sessionId}");
        if (response.StatusCode == HttpStatusCode.BadRequest) await CreateSession(sessionId);
    }

    public async Task<List<SongData>> GetSongsFromSession(string sessionId)
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-songs?sessionId={sessionId}");
        if (!response.IsSuccessStatusCode) return null;
        List<SongData> songs = new List<SongData>();
        try
        {
            songs = JArray.Parse(await response.Content.ReadAsStringAsync()).ToObject<List<SongData>>();
        } catch (JsonReaderException) {}
        return songs ?? new List<SongData>();
    }
    
    public async Task<List<SongData>> GetFilteredSongsFromSession(string sessionId)
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-filtered-songs?sessionId={sessionId}");
        if (!response.IsSuccessStatusCode) return null;
        List<SongData> songs = new List<SongData>();
        try
        {
            songs = JArray.Parse(await response.Content.ReadAsStringAsync()).ToObject<List<SongData>>();
        } catch (JsonReaderException) {}
        return songs ?? new List<SongData>();
    }

    public async Task<List<IFilterViewModel>> GetFiltersFromSession(string sessionId)
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-filters?sessionId={sessionId}");
        if (!response.IsSuccessStatusCode) return null;
        var filterDict = JObject.Parse(await response.Content.ReadAsStringAsync()).ToObject<Dictionary<TrackFilter, List<object>>>();
        if (filterDict == null) return null;
        List<IFilterViewModel> filters = new List<IFilterViewModel>();
        foreach (KeyValuePair<TrackFilter, List<object>> filterkvp in filterDict)
        {
            IFilterViewModel filter = new PlaylistTextFilterViewModel(filterkvp.Key);
            switch (filterkvp.Key)
            {
                case TrackFilter.Genre:
                    filter = new PlaylistTextFilterViewModel(filterkvp.Key, filterkvp.Value);
                    break;
                case TrackFilter.Popularity:
                case TrackFilter.Danceability:
                case TrackFilter.Energy:
                case TrackFilter.Positivity:
                    filter = new PlaylistRangeFilterViewModel(filterkvp.Key, filterkvp.Value);
                    break;
                case TrackFilter.Tempo:
                    filter = new PlaylistNumberFilterViewModel(filterkvp.Key, filterkvp.Value); 
                    break;
            }
            filters.Add(filter);
        }

        return filters;
    }

    public async Task SetFiltersOfSession()
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/set-filters?sessionId={StorageHandler.CollaborationSession}", new StringContent(JsonConvert.SerializeObject(DeserializeFilters(PlaylistCreatorPageModel.Filters)), Encoding.UTF8, "application/json"));
    }
    
    public async Task CreateSession()
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/create-session?sessionId={sessionId}", new StringContent(string.Empty, Encoding.UTF8, "application/json"));
    }

    public async Task SetSongsOfSession(string sessionId, List<string> songIds)
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/set-songs?sessionId={sessionId}", new StringContent(JsonConvert.SerializeObject(songIds), Encoding.UTF8, "application/json"));
        // filter songs (the filtered version will be get on next ui refresh)
        await TriggerFiltering(sessionId);
        // stopping loading animation
        PlaylistCreationSonglistViewModel.PlaylistFinishedFiltering();
    }

    public async Task TriggerFiltering(string sessionId)
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/filter-songs?sessionId={sessionId}", new StringContent(string.Empty));
    }

    private Dictionary<TrackFilter, List<object>> DeserializeFilters(List<IFilterViewModel> filterInputs)
    {
        var filters = new Dictionary<TrackFilter, List<object>>();
        foreach (var filterInput in filterInputs)
        {
            KeyValuePair<TrackFilter, List<object>> filter = new KeyValuePair<TrackFilter, List<object>>();
            switch (filterInput)
            {
                case PlaylistTextFilterViewModel textFilter:
                    filter = new KeyValuePair<TrackFilter, List<object>>(textFilter.GetTrackFilter(), new List<object>{textFilter.GetGuid(), textFilter.GenreName});
                    break;
                case PlaylistRangeFilterViewModel rangeFilter:
                    filter = new KeyValuePair<TrackFilter, List<object>>(rangeFilter.GetTrackFilter(), new List<object>{rangeFilter.GetGuid(), rangeFilter.SelectedFilterOption, rangeFilter.SliderValue});
                    break;
                case PlaylistNumberFilterViewModel numberFilter:
                    filter = new KeyValuePair<TrackFilter, List<object>>(numberFilter.GetTrackFilter(), new List<object>{numberFilter.GetGuid(), numberFilter.SelectedFilterOption, numberFilter.EnteredNumber});
                    break;
            }
            
            filters.Add(filter.Key, filter.Value);
        }

        return filters;
    }
}
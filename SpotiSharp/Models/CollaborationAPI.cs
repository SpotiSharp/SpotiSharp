using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotiSharp.Interfaces;
using SpotiSharp.ViewModels;
using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend;

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

    public async Task GetSession()
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get?sessionId={StorageHandler.CollaborationSession}");
        if (response.StatusCode == HttpStatusCode.BadRequest) await CreateSession();
    }

    public async Task<List<SongData>> GetSongsFromSession()
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-songs?sessionId={StorageHandler.CollaborationSession}");
        if (!response.IsSuccessStatusCode) return null;
        List<SongData> songs = new List<SongData>();
        try
        {
            songs = JArray.Parse(await response.Content.ReadAsStringAsync()).ToObject<List<SongData>>();
        } catch (JsonReaderException) {}
        return songs ?? new List<SongData>();
    }
    
    public async Task<List<SongData>> GetFilteredSongsFromSession()
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-filtered-songs?sessionId={StorageHandler.CollaborationSession}");
        if (!response.IsSuccessStatusCode) return null;
        List<SongData> songs = new List<SongData>();
        try
        {
            songs = JArray.Parse(await response.Content.ReadAsStringAsync()).ToObject<List<SongData>>();
        } catch (JsonReaderException) {}
        return songs ?? new List<SongData>();
    }

    public async Task<List<List<object>>?> GetFiltersFromSession()
    {
        var response = await _client.GetAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/get-filters?sessionId={StorageHandler.CollaborationSession}");
        if (!response.IsSuccessStatusCode) return null;
        return JArray.Parse(await response.Content.ReadAsStringAsync()).ToObject<List<List<object>>>();
    }

    public async Task SetFiltersOfSession()
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/set-filters?sessionId={StorageHandler.CollaborationSession}", new StringContent(JsonConvert.SerializeObject(DeserializeFilters(PlaylistCreatorPageModel.Filters)), Encoding.UTF8, "application/json"));
    }
    
    public async Task CreateSession()
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/create-session?sessionId={StorageHandler.CollaborationSession}", new StringContent(string.Empty, Encoding.UTF8, "application/json"));
    }

    public async Task SetSongsOfSession(List<string> songIds)
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/set-songs?sessionId={StorageHandler.CollaborationSession}", new StringContent(JsonConvert.SerializeObject(songIds), Encoding.UTF8, "application/json"));
        // filter songs (the filtered version will be get on next ui refresh)
        await TriggerFiltering();
        // stopping loading animation
        PlaylistCreationSonglistViewModel.PlaylistFinishedFiltering();
    }

    public async Task TriggerFiltering()
    {
        await _client.PostAsync($"{StorageHandler.CollaborationHostAddress}/CollaborationSession/filter-songs?sessionId={StorageHandler.CollaborationSession}", new StringContent(string.Empty));
    }

    private List<List<object>> DeserializeFilters(List<IFilterViewModel> filterInputs)
    {
        var filters = new List<List<object>>();
        foreach (var filterInput in filterInputs)
        {
            List<object> filter = new List<object>();
            switch (filterInput)
            {
                case PlaylistTextFilterViewModel textFilter:
                    filter = new List<object>(new List<object>{textFilter.GetTrackFilter(), textFilter.GetGuid(), textFilter.GenreName});
                    break;
                case PlaylistRangeFilterViewModel rangeFilter:
                    filter = new List<object>(new List<object>{rangeFilter.GetTrackFilter(), rangeFilter.GetGuid(), rangeFilter.SelectedFilterOption, rangeFilter.SliderValue});
                    break;
                case PlaylistNumberFilterViewModel numberFilter:
                    filter = new List<object>(new List<object>{numberFilter.GetTrackFilter(), numberFilter.GetGuid(), numberFilter.SelectedFilterOption, numberFilter.EnteredNumber});
                    break;
            }
            
            filters.Add(filter);
        }

        return filters;
    }
}
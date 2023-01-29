using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharp.ViewModels;
using SpotiSharpBackend;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Models;

public class CollaborationSessionConnection
{
    private static CollaborationSessionConnection _collaborationSessionConnection;
    
    public static CollaborationSessionConnection Instance => _collaborationSessionConnection ??= new CollaborationSessionConnection();
    
    private CollaborationSessionConnection()
    {
        UiLoop.Instance.OnRefreshUi += RefreshValues;
        PlaylistCreatorPageModel.OnSongListChange += SendChangedSongs;
    }

    private void RefreshValues()
    {
        if (!StorageHandler.IsUsingCollaborationHost) return;

        ApplySongsFromSession();
        ApplyFiltersFromSession();
    }
    
    private void ApplySongsFromSession()
    {
        List<FullTrack>? songs = CollaborationAPI.Instance?.GetSongsFromSession().Result?.Select(item => item.FullTrack).ToList();
        List<FullTrack>? songsFiltered = CollaborationAPI.Instance?.GetFilteredSongsFromSession().Result?.Select(item => item.FullTrack).ToList();
        if (songs == null || songsFiltered == null) return;

        if (!PlaylistCreatorPageModel.SongsEqual(PlaylistCreatorPageModel.UnfilteredSongs, songs)) PlaylistCreatorPageModel.UnfilteredSongsOnlyUiUpdate = songs;
        if (!PlaylistCreatorPageModel.SongsEqual(PlaylistCreatorPageModel.CurrentFilteredSongs, songsFiltered)) PlaylistCreatorPageModel.CurrentFilteredSongs = songsFiltered;
    }

    private void ApplyFiltersFromSession()
    { 
        List<List<object>>? incomingFilters = CollaborationAPI.Instance?.GetFiltersFromSession().Result;
        if (incomingFilters == null) return;
        IEnumerable<Guid> incomingGuids = incomingFilters.Select(kvp => new Guid(kvp[1].ToString()));
        List<IFilterViewModel> existingFilters = PlaylistCreatorPageModel.Filters;

        var filtersToRemove = new List<IFilterViewModel>();
        // check if local filters still exist
        foreach (var existingFilter in existingFilters)
        {
            if (incomingGuids.Contains(existingFilter.GetGuid()))
            {
                // update local filter values
                var incomingFilter = incomingFilters.First(inf => new Guid(inf[1].ToString()).Equals(existingFilter.GetGuid()));
                existingFilter.SyncValues(incomingFilter.Skip(2).ToList());
            }
            else
            {
                filtersToRemove.Add(existingFilter);
            }
            
        }
        foreach (var filterToRemove in filtersToRemove)
        {
            filterToRemove.RemoveFilter();
        }
        
        // check if all external filters exist locally
        IEnumerable<Guid> existingGuids = existingFilters.Select(fvm => fvm.GetGuid());
        foreach (var filter in incomingFilters)
        {
            if (existingGuids.Contains(new Guid(filter[1].ToString()))) continue;
            PlaylistCreatorPageViewModel.InvokeAddFilter(Enum.Parse<TrackFilter>(filter[0].ToString()), new Guid(filter[1].ToString()), filter.Skip(2).ToList());
        }
    }
    
    private void SendChangedSongs()
    {
        CollaborationAPI.Instance?.SetSongsOfSession(PlaylistCreatorPageModel.UnfilteredSongs.Select(ft => ft.Id).ToList());
    }
}
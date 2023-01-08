﻿using SpotifyAPI.Web;
using SpotiSharp.Interfaces;
using SpotiSharpBackend;

namespace SpotiSharp.Models;

public class CollaborationSessionConnection
{
    private static CollaborationSessionConnection _collaborationSessionConnection;
    
    public static CollaborationSessionConnection Instance => _collaborationSessionConnection ??= new CollaborationSessionConnection();
    
    private CollaborationSessionConnection()
    {
        UiLoop.Instance.OnRefreshUi += RefreshValues;
    }

    private void RefreshValues()
    {
        if (!StorageHandler.IsUsingCollaborationHost) return;

        ApplySongsFromSession();
        ApplyFiltersFromSession();
    }
    
    private void ApplySongsFromSession()
    {
        List<FullTrack>? songs = CollaborationAPI.Instance?.GetSongsFromSession(StorageHandler.CollaborationSession).Result?.Select(item => item.FullTrack).ToList();
        List<FullTrack>? songsFiltered = CollaborationAPI.Instance?.GetFilteredSongsFromSession(StorageHandler.CollaborationSession).Result?.Select(item => item.FullTrack).ToList();
        if (songs == null || songsFiltered == null) return;
        // TODO: implement Equals to so that the ui is only updated if the list has actually changed.
        if (!PlaylistCreatorPageModel.UnfilteredSongs.Equals(songs)) PlaylistCreatorPageModel.UnfilteredSongs = songs;
        if (!PlaylistCreatorPageModel.CurrentFilteredSongs.Equals(songsFiltered)) PlaylistCreatorPageModel.CurrentFilteredSongs = songsFiltered;
    }

    private void ApplyFiltersFromSession()
    { 
        List<IFilterViewModel>? filters = CollaborationAPI.Instance?.GetFiltersFromSession(StorageHandler.CollaborationSession).Result;
        if (filters == null) return;
    }
}
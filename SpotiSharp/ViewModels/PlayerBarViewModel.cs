using System.Windows.Input;
using SpotifyAPI;
using SpotifyAPI.Web;

namespace SpotiSharp.ViewModels;

public class PlayerBarViewModel : BaseViewModel
{
    private static PlayerBarViewModel _playerBarViewModel;
    public static PlayerBarViewModel Instance => _playerBarViewModel ??= new PlayerBarViewModel();
    
    private string _songName;

    public string SongName
    {
        get { return _songName; }
        set { SetProperty(ref _songName, value); }
    }
    
    private string _songImageURL;

    public string SongImageURL
    {
        get { return _songImageURL; }
        set { SetProperty(ref _songImageURL, value); }
    }

    private PlayerBarViewModel()
    { 
        _playerBarViewModel = this;
        TogglePlaying = new Command(TogglePlayingFunc);
        SongBack = new Command(SongBackFunc);
        SongSkip = new Command(SongSkipFunc);
        ChangeRepeat = new Command(ChangeRepeatFunc);
        ChangeShuffle = new Command(ChangeShuffleFunc);
        UiLoop.Instance.OnRefreshUi += RefreshPlayerValues;
    }

    private void RefreshPlayerValues()
    {
        CurrentlyPlaying? currentPlayingSong;
        try
        {
            currentPlayingSong = APICaller.Instance?.GetCurrentSong();
            if (currentPlayingSong == null) return;
        }
        catch (UnauthorizedAccessException)
        {
            SongName = "Unauthorized";
            return;
        }

        switch (currentPlayingSong.Item)
        {
            case FullTrack fullTrack:
            {
                SongName = fullTrack.Name;
                SongImageURL = fullTrack.Album.Images.ElementAtOrDefault(0)?.Url ?? string.Empty;
                break;
            }
            case FullEpisode fullEpisode:
            {
                SongName = fullEpisode.Name;
                SongImageURL = fullEpisode.Images.ElementAtOrDefault(0)?.Url ?? string.Empty;
                break;
            }
        }
    }
    
    private void TogglePlayingFunc()
    {
        APICaller.Instance?.TogglePlaybackStatus();
    }
    
    private void SongBackFunc()
    {
        if (APICaller.Instance?.SkipToPreviousSong() ?? false) RefreshPlayerValues();
    }
    
    private void SongSkipFunc()
    {
        if (APICaller.Instance?.SkipToNextSong() ?? false) RefreshPlayerValues();
    }
    
    private void ChangeRepeatFunc()
    {
        APICaller.Instance?.ChangePlaybackRepeatType();
    }
    
    private void ChangeShuffleFunc()
    {
        APICaller.Instance?.TogglePlaybackShuffle();
    }
    
    public ICommand TogglePlaying { private set; get; }
    public ICommand SongBack { private set; get; }
    public ICommand SongSkip { private set; get; }
    public ICommand ChangeRepeat { private set; get; }
    public ICommand ChangeShuffle { private set; get; }
}
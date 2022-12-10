using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpotifyAPI;
using SpotifyAPI.Web;

namespace SpotiSharp.ViewModels;

public class PlayerBarViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

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

    public PlayerBarViewModel()
    {
        TogglePlaying = new Command(TogglePlayingFunc);
        SongBack = new Command(SongBackFunc);
        SongSkip = new Command(SongSkipFunc);
        ChangeRepeat = new Command(ChangeRepeatFunc);
        ChangeShuffle = new Command(ChangeShuffleFunc);

        CurrentlyPlaying currentPlayingSong;
        try
        {
            currentPlayingSong = Player.GetCurrentSong();
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
                SongImageURL = fullTrack.Album.Images[0].Url;
                break;
            }
            case FullEpisode fullEpisode:
            {
                SongName = fullEpisode.Name;
                SongImageURL = fullEpisode.Images[0].Url;
                break;
            }
        }
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    private void TogglePlayingFunc()
    {
        Player.TogglePlaybackStatus();
    }
    
    private void SongBackFunc()
    {
        Player.SkipToPreviousSong();
    }
    
    private void SongSkipFunc()
    {
        Player.SkipToNextSong();
    }
    
    private void ChangeRepeatFunc()
    {
        Player.ChangePlaybackRepeatType();
    }
    
    private void ChangeShuffleFunc()
    {
        Player.TogglePlaybackShuffle();
    }
    
    public ICommand TogglePlaying { private set; get; }
    public ICommand SongBack { private set; get; }
    public ICommand SongSkip { private set; get; }
    public ICommand ChangeRepeat { private set; get; }
    public ICommand ChangeShuffle { private set; get; }
}
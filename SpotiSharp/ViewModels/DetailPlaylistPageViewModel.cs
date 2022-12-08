using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public class DetailPlaylistPageViewModel : INotifyPropertyChanged
{
    private string _playlistId;

    public string PlaylistId
    {
        get { return _playlistId; }
        set
        {
            SetProperty(ref _playlistId, value);
            RefreshPlaylistInfo();
        }
    }
    
    private string _imageURL;

    public string ImageURL
    {
        get { return _imageURL; }
        set { SetProperty(ref _imageURL, value); }
    }
    
    private string _playlistName;

    public string PlaylistName
    {
        get { return _playlistName; }
        set { SetProperty(ref _playlistName, value); }
    }
    
    private string _playlistDescription;

    public string PlaylistDescription
    {
        get { return _playlistDescription; }
        set { SetProperty(ref _playlistDescription, value); }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    private DetailPlaylistModel _detailPlaylistModel;
    
    
    public DetailPlaylistPageViewModel()
    {
        _detailPlaylistModel = new DetailPlaylistModel();
    }

    private void RefreshPlaylistInfo()
    {
        if (PlaylistId == null) return;
        ImageURL = _detailPlaylistModel.GetPlaylistImageURL(PlaylistId);
        PlaylistName = _detailPlaylistModel.GetPlaylistName(PlaylistId);
        PlaylistDescription = _detailPlaylistModel.GetPlaylistDescription(PlaylistId);
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
}
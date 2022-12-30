using System.Windows.Input;

namespace SpotiSharp.ViewModels;

public class HeaderViewModel : BaseViewModel
{
    public HeaderViewModel()
    {
        HomeClicked = new Command(SwitchToHome);
        PlaylistCreationClicked = new Command(SwitchToPlaylistCreation);
        ViewPlaylistsClicked = new Command(SwitchToViewPlaylists);
        AuthenticationClicked = new Command(SwitchToAuthentication);
    }

    private async void SwitchToHome()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void SwitchToPlaylistCreation()
    {
        await Shell.Current.GoToAsync("//PlaylistCreatorPage");
    }

    private async void SwitchToViewPlaylists()
    {
        await Shell.Current.GoToAsync("//ManagePlayListsPage");
    }

    private async void SwitchToAuthentication()
    {
        await Shell.Current.GoToAsync("//AuthenticationPage");
    }

    public ICommand HomeClicked { private set; get; }
    public ICommand PlaylistCreationClicked { private set; get; }
    public ICommand ViewPlaylistsClicked { private set; get; }
    public ICommand AuthenticationClicked { private set; get; }
}
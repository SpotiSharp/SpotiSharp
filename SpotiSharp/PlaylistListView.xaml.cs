using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class PlaylistListView : ContentView
{
    public PlaylistListView()
    {
        InitializeComponent();
        BindingContext = new PlaylistListViewModel();
        
        MainListView.ItemTapped += (sender, args) =>
        {
            if (BindingContext is PlaylistListViewModel playlistListViewModel) playlistListViewModel.GoToPlaylistDetail();
        };
    }
}
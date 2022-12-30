using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class PlaylistCreationSonglistView : ContentView
{
    public PlaylistCreationSonglistView()
    {
        InitializeComponent();
        BindingContext = new PlaylistCreationSonglistViewModel();
    }
}
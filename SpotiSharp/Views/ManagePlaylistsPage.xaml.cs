using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class ManagePlaylistsPage : ContentPage
{
    public ManagePlaylistsPage()
    {
        InitializeComponent();
        BindingContext = new ManagePlaylistsPageViewModel();
    }
}
using SpotiSharp.ViewModels;
using SpotiSharp.Views;

namespace SpotiSharp;

public partial class ManagePlaylistsPage : BasePage
{
    public ManagePlaylistsPage()
    {
        InitializeComponent();
        BindingContext = new ManagePlaylistsPageViewModel();
    }
}
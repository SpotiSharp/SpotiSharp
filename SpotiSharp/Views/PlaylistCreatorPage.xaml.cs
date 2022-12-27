using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class PlaylistCreatorPage : BasePage
{
    public PlaylistCreatorPage()
    {
        InitializeComponent();
        BindingContext = new PlaylistCreatorPageViewModel();
    }
}
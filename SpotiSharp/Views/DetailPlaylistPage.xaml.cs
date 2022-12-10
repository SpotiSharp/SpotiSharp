using SpotiSharp.ViewModels;
using SpotiSharp.Views;

namespace SpotiSharp;

public partial class DetailPlaylistPage : BasePage, IQueryAttributable
{
    public DetailPlaylistPage()
    {
        InitializeComponent();
        BindingContext = new DetailPlaylistPageViewModel();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is not DetailPlaylistPageViewModel bindingContext) return;
        bindingContext.PlaylistId = query["PlaylistId"] as string;
        SongsListView.PlayListId = bindingContext.PlaylistId;
    }
}
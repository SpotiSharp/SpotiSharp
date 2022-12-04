using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class DetailPlaylistPage : ContentPage, IQueryAttributable
{
    public DetailPlaylistPage()
    {
        InitializeComponent();
        BindingContext = new DetailPlaylistViewModel();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is not DetailPlaylistViewModel bindingContext) return;
        bindingContext.PlaylistId = query["PlaylistId"] as string;
        SongsListView.PlayListId = bindingContext.PlaylistId;
    }
}
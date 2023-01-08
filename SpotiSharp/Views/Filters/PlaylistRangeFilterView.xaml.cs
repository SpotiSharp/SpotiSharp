using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistRangeFilterView : ContentView
{
    public PlaylistRangeFilterView(TrackFilter trackFilter)
    {
        InitializeComponent();
        BindingContext = new PlaylistRangeFilterViewModel(trackFilter);
    }
}
using SpotiSharp.Enums;
using SpotiSharp.ViewModels.Filters;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistRangeFilterView : ContentView
{
    public PlaylistRangeFilterView(TrackFilter trackFilter)
    {
        InitializeComponent();
        BindingContext = new PlaylistRangeFilterViewModel(trackFilter);
    }
}
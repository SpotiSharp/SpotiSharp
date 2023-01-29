using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistRangeFilterView : ContentView
{
    public PlaylistRangeFilterView(TrackFilter trackFilter, Guid guid, List<object> parameters)
    {
        InitializeComponent();
        BindingContext = new PlaylistRangeFilterViewModel(trackFilter, guid, parameters);
    }
}
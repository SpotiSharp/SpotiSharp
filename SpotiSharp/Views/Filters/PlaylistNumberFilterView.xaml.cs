using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistNumberFilterView : ContentView
{
    public PlaylistNumberFilterView(TrackFilter trackFilter)
    {
        InitializeComponent();
        BindingContext = new PlaylistNumberFilterViewModel(trackFilter);
    }
}
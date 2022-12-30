using SpotiSharp.Enums;
using SpotiSharp.ViewModels.Filters;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistNumberFilterView : ContentView
{
    public PlaylistNumberFilterView(TrackFilter trackFilter)
    {
        InitializeComponent();
        BindingContext = new PlaylistNumberFilterViewModel(trackFilter);
    }
}
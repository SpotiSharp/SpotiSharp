using SpotiSharp.Enums;
using SpotiSharp.ViewModels.Filters;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistTextFilterView : ContentView
{
    public PlaylistTextFilterView(TrackFilter trackFilter)
    {
        InitializeComponent();
        BindingContext = new PlaylistTextFilterViewModel(trackFilter);
    }
}
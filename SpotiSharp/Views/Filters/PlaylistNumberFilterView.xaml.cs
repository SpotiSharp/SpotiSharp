using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistNumberFilterView : ContentView
{
    public PlaylistNumberFilterView(TrackFilter trackFilter, Guid guid, List<object> parameters)
    {
        InitializeComponent();
        BindingContext = new PlaylistNumberFilterViewModel(trackFilter, guid, parameters);
    }
}
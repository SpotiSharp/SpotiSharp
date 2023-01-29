using SpotiSharp.ViewModels.Filters;
using SpotiSharpBackend.Enums;

namespace SpotiSharp.Views.Filters;

public partial class PlaylistTextFilterView : ContentView
{
    public PlaylistTextFilterView(TrackFilter trackFilter, Guid guid, List<object> parameters)
    {
        InitializeComponent();
        BindingContext = new PlaylistTextFilterViewModel(trackFilter, guid, parameters);
    }
}
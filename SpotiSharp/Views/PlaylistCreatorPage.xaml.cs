using SpotiSharp.Enums;
using SpotiSharp.Interfaces;
using SpotiSharp.ViewModels;
using SpotiSharp.ViewModels.Filters;
using SpotiSharp.Views.Filters;

namespace SpotiSharp.Views;

public partial class PlaylistCreatorPage : BasePage
{
    private List<ContentView> FilterViews = new List<ContentView>();

    public PlaylistCreatorPage()
    {
        InitializeComponent();
        BindingContext = new PlaylistCreatorPageViewModel();
        PlaylistCreatorPageViewModel.OnAddingFilter += AddFilterElement;
        BaseFilter.OnFilterRemove += RemoveFilterElement;
    }

    private void AddFilterElement(TrackFilter trackFilter)
    {
        ContentView playlistFilterView = null;
        switch (trackFilter)
        {
            case TrackFilter.Genre:
                playlistFilterView = new PlaylistTextFilterView(trackFilter);
                break;
            case TrackFilter.Popularity:
            case TrackFilter.Danceability:
            case TrackFilter.Energy:
            case TrackFilter.Positivity:
                playlistFilterView = new PlaylistRangeFilterView(trackFilter);
                break;
            case TrackFilter.Tempo:
                playlistFilterView = new PlaylistNumberFilterView(trackFilter); 
                break;
        }
        if (playlistFilterView == null) return;
        Application.Current.Dispatcher.Dispatch(() =>
        {
            FilterLayout.Add(playlistFilterView);
        });
        FilterViews.Add(playlistFilterView);
    }

    private void RemoveFilterElement(int index)
    {
        Application.Current.Dispatcher.Dispatch(() =>
        {
            FilterLayout.RemoveAt(index);
            FilterViews.RemoveAt(index);
        });
    }
}
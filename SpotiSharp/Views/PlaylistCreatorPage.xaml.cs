using SpotiSharp.Enums;
using SpotiSharp.ViewModels;
using SpotiSharp.Views.Filters;

namespace SpotiSharp.Views;

public partial class PlaylistCreatorPage : BasePage
{
    public PlaylistCreatorPage()
    {
        InitializeComponent();
        BindingContext = new PlaylistCreatorPageViewModel();
        PlaylistCreatorPageViewModel.OnAddingFilter += AddFilterElement;

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
    }
}
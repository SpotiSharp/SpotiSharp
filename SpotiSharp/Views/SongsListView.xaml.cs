using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class SongsListView : ContentView
{
    public static readonly BindableProperty PlayListIdProperty = BindableProperty.Create(nameof(PlayListId), typeof(string), typeof(SongsListView), string.Empty);
    
    public string PlayListId
    {
        get => (string)GetValue(PlayListIdProperty);
        set
        {
            SetValue(PlayListIdProperty, value);
            var songsListViewModel = BindingContext as SongsListViewModel;
            songsListViewModel?.OnPlayListIdRefresh(value);
        }
    }

    public SongsListView()
    {
        InitializeComponent();
        BindingContext = new SongsListViewModel();
        
        MainListView.ItemTapped += (sender, args) =>
        {
            if (BindingContext is SongsListViewModel songsListViewModel) songsListViewModel.ClickSong(args.Item);
        };
    }
}
using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class PlayerBarView : ContentView
{
    public PlayerBarView()
    {
        InitializeComponent();
        BindingContext = new PlayerBarViewModel();
    }
}
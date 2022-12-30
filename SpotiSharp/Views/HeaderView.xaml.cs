using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();
        BindingContext = new HeaderViewModel();
    }
}
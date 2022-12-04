using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class ManagePlayListsPage : ContentPage
{
    public ManagePlayListsPage()
    {
        InitializeComponent();
        BindingContext = new ManagePlayListsViewModel();
    }
}
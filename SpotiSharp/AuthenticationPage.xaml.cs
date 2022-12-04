using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class AuthenticationPage : ContentPage
{
    public AuthenticationPage()
    {
        InitializeComponent();
        BindingContext = new AuthenticationPageViewModel();
    }
}
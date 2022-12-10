using SpotiSharp.ViewModels;
using SpotiSharp.Views;

namespace SpotiSharp;

public partial class AuthenticationPage : BasePage
{
    public AuthenticationPage()
    {
        InitializeComponent();
        BindingContext = new AuthenticationPageViewModel();
    }
}
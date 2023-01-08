using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class SettingsPage : BasePage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsPageViewModel();
    }
}
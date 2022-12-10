using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public class BasePage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as BaseViewModel)?.OnAppearing();
    }
}
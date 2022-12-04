using SpotiSharp.ViewModels;

namespace SpotiSharp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}
}


using SpotiSharp.ViewModels;
using SpotiSharp.Views;

namespace SpotiSharp;

public partial class MainPage : BasePage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}
}


using SpotiSharp.Models;
using SpotiSharp.ViewModels;
using SpotiSharp.Views;

namespace SpotiSharp;

public partial class MainPage : BasePage
{
	public MainPage()
	{
		// calling constructor of BackendConnector to load SecureStorage
		_ = BackendConnector.Instance;
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}
}


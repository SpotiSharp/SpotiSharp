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
		// calling constructor of CollaborationSessionConnection to add it's actions to the ui loop
		_ = CollaborationSessionConnection.Instance;
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}
}


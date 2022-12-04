namespace SpotiSharp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute("DetailPlaylistPage", typeof(DetailPlaylistPage));
	}
}

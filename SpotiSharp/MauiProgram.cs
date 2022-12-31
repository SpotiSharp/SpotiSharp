using Syncfusion.Maui.Core.Hosting;

namespace SpotiSharp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
		var uiRefresherThread = new Thread(UiLoop.Instance.Loop);
		uiRefresherThread.Start();
#if DEBUG
#endif

		return builder.Build();
	}
}

using SpotiSharp.Models;
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
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9iS3xVdEZiWn5Yd3dUQw==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xSdEdlW3xfc3VTQWRZVA==;ORg4AjUWIQA/Gnt2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0dhX31dcXdUQGdaU0E=;ODIxMzI0QDMyMzAyZTM0MmUzMFRUOXJPUzhJd0dvc0gzUmFiektBMlRIaEhNNjNMVTVPZkR3eEFKTStGSXM9;ODIxMzI1QDMyMzAyZTM0MmUzMEdwblI0d25rczJVU2VDKzVoKzBiR2pLTGtLS3BhOUZpSDFhQ1VBSjJWcWM9;NRAiBiAaIQQuGjN/V0Z+WE9EaFxKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdERhW3lcc3VWR2ZUUkFz;ODIxMzI3QDMyMzAyZTM0MmUzME9pSFhoTjVYTStiV2xiaUYxMDFSRUNvc3JKc2VxcGovK2NJQm9kV0pKRDA9;ODIxMzI4QDMyMzAyZTM0MmUzMG8xVmpKOWFrRDVXT1UwOEZiZGtOWUFtSTc0dlZ6MHRYODhwV3N4ZVFwVEU9;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0dhX31dcXdUQGleUEE=;ODIxMzMwQDMyMzAyZTM0MmUzME43Tno3MjRFUWVkZEY3UURZb2d5YlJRWE1ieGNJVkl2QzhwbC9IbkkzNGc9;ODIxMzMxQDMyMzAyZTM0MmUzMFAweU9qVnhRT0FyTFZqNURTV1JqNWRBZXo4b3FUcWhqdVJnMGxVMStmYzg9;ODIxMzMyQDMyMzAyZTM0MmUzME9pSFhoTjVYTStiV2xiaUYxMDFSRUNvc3JKc2VxcGovK2NJQm9kV0pKRDA9");
		var uiRefresherThread = new Thread(UiLoop.Instance.Loop);
		uiRefresherThread.Start();
#if DEBUG
#endif

		return builder.Build();
	}
}

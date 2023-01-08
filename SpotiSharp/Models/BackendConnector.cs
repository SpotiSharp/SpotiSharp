using SpotiSharpBackend;

namespace SpotiSharp.Models;

public class BackendConnector
{
    private static BackendConnector _backendConnector;
    
    public static BackendConnector Instance => _backendConnector ??= new BackendConnector();
    
    private BackendConnector()
    {
        StorageHandler.ClientId = SecureStorage.Default.GetAsync("clientId").Result;
        StorageHandler.RefreshToken = SecureStorage.Default.GetAsync("refreshToken").Result;
        MauiConnector.OnOpenBrowser += OpenBrowser;
        StorageHandler.OnDataChange += StoreInSecureStorage;
    }

    private static async void OpenBrowser(Uri uri)
    {
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private static void StoreInSecureStorage(string key, string value)
    {
        SecureStorage.Default.SetAsync(key, value);
    }
        
}
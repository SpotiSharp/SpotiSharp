using System.Windows.Input;
using SpotiSharp.Models;
using SpotiSharpBackend;

namespace SpotiSharp.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private bool _isUsingCollaborationHost;

    public bool IsUsingCollaborationHost
    {
        get { return _isUsingCollaborationHost; }
        set { SetProperty(ref _isUsingCollaborationHost, value); }
    }
    
    private string _collaborationHostAddress;

    public string CollaborationHostAddress
    {
        get { return _collaborationHostAddress; }
        set { SetProperty(ref _collaborationHostAddress, value); }
    }
    
    private string _collaborationSession;

    public string CollaborationSession
    {
        get { return _collaborationSession; }
        set { SetProperty(ref _collaborationSession, value); }
    }

    public SettingsPageViewModel()
    {
        ApplySettings = new Command(() =>
        {
            StorageHandler.IsUsingCollaborationHost = IsUsingCollaborationHost;
            StorageHandler.CollaborationHostAddress = CollaborationHostAddress;
            StorageHandler.CollaborationSession = CollaborationSession;
            if (IsUsingCollaborationHost) CollaborationAPI.Instance?.CreateSession(CollaborationSession);
            // TODO: else clear current state of playlist creation.
            });
    }
    
    public ICommand ApplySettings { private set; get; }
}
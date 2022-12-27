namespace SpotiSharp;

public delegate void RefreshUi();

public class UiLoop
{
    private static UiLoop _uiLoop;
    public static UiLoop Instance => _uiLoop ??= new UiLoop();
    
    private const int UI_REFRESH_INTERVAL_IN_MILLI = 500;
    
    public event RefreshUi OnRefreshUi;
    
    private UiLoop() {}

    public void Loop()
    {
        while (true)
        {
            OnLoopInterval();
            Thread.Sleep(UI_REFRESH_INTERVAL_IN_MILLI);
        }
    }
    
    protected virtual void OnLoopInterval()
    {
        OnRefreshUi?.Invoke(); 
    }
}
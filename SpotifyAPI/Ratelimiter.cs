namespace SpotifyAPI;

public static class Ratelimiter
{
    private const int MAX_API_CALLS_PER_SCOND = 5;
    private static int _currentCallCount;

    static Ratelimiter()
    {
        var thread = new Thread(ResetLimit);
        thread.Start();
    }

    private static void ResetLimit()
    {
        while (true)
        {
            Thread.Sleep(500);
            _currentCallCount = 0;
        }
    }

    public static bool RequestCall()
    {
        if (_currentCallCount >= MAX_API_CALLS_PER_SCOND) return false;
        _currentCallCount++;
        return true;
    }

    public static bool CanRequestCall()
    {
        return _currentCallCount < MAX_API_CALLS_PER_SCOND;
    }

}
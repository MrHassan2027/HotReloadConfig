namespace HotReloadConfig;

public class Debouncer(TimeSpan delay)
{
    private CancellationTokenSource? _cts;

    public void Invoke(Action action)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        Task.Delay(delay, token).ContinueWith(t =>
        {
            if (!t.IsCanceled) action();
        }, TaskScheduler.Default);
    }
}

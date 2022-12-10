namespace MyHomeAutomation.WebApi;

public abstract class PeriodTaskBase
{
    private readonly TimeSpan _interval;
    private  Task? _timerTask;
    private readonly PeriodicTimer _periodicTimer;
    private readonly CancellationTokenSource _cts = new();

    public PeriodTaskBase(TimeSpan interval)
    {
        _periodicTimer = new PeriodicTimer(interval);
    }
    
    public void Start()
    {
        _timerTask = DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        try
        {
            while (await _periodicTimer.WaitForNextTickAsync(_cts.Token))
            {
                await DoWhatYouNeed();
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
    
    public async Task Stop()
    {
        if (_timerTask is null)
        {
            return;
        }

        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
        Console.WriteLine("PeriodTaskBase stopped");
    }

    protected abstract Task DoWhatYouNeed();
}
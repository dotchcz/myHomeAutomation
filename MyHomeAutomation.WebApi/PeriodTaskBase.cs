#nullable enable
namespace MyHomeAutomation.WebApi;

public abstract class PeriodTaskBase
{
    private readonly ILogger _logger;
    private  Task? _timerTask;
    private readonly PeriodicTimer _periodicTimer;
    private readonly CancellationTokenSource _cts = new();

    public PeriodTaskBase(ILogger logger, TimeSpan interval)
    {
        _logger = logger;
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
                try
                {
                    await DoWhatYouNeed();
                }
                catch (TaskCanceledException)
                {
                    
                }
                catch (OperationCanceledException)
                {
                }
                
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
        _logger.LogInformation("PeriodTaskBase stopped");
    }

    protected abstract Task DoWhatYouNeed();
}
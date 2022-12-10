namespace MyHomeAutomation.WebApi;

public class InterrogationService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private InterrogationTask _interrogationTask;

    public InterrogationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _interrogationTask = new InterrogationTask(TimeSpan.FromMilliseconds(3000), _serviceScopeFactory);
        _interrogationTask.Start();
        Console.WriteLine("Hello from the other side!");
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Service stopped");
        await _interrogationTask.Stop();
    }
}
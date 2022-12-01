namespace MyHomeAutomation.WebApi;

public class RegulatoryService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private RegulatoryTask _regulatoryTask;

    public RegulatoryService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _regulatoryTask = new RegulatoryTask(TimeSpan.FromMilliseconds(5000), _serviceScopeFactory);
        _regulatoryTask.Start();
        Console.WriteLine("Hello from the other side!");
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Service stopped");
        await _regulatoryTask.Stop();
    }
}
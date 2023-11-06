namespace MyHomeAutomation.WebApi;

public class RegulatoryService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private RegulatoryTask _regulatoryTask;

    public RegulatoryService(ILogger<RegulatoryService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _regulatoryTask = new RegulatoryTask(_logger,TimeSpan.FromMilliseconds(5000), _serviceScopeFactory);
        _regulatoryTask.Start();
        _logger.LogInformation("Hello from the other side!");
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopped");
        await _regulatoryTask.Stop();
    }
}
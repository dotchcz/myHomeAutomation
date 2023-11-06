namespace MyHomeAutomation.WebApi;

public class InterrogationService : IHostedService
{
    private readonly ILogger<InterrogationService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private InterrogationTask _interrogationTask;

    public InterrogationService(ILogger<InterrogationService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _interrogationTask = new InterrogationTask(_logger, TimeSpan.FromMilliseconds(3000), _serviceScopeFactory);
        _interrogationTask.Start();
        _logger.LogInformation("Hello from the other side!");
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopped");
        await _interrogationTask.Stop();
    }
}
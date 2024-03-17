namespace MyHomeAutomation.WebApi;

public class BoilerControlService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private BoilerControlTask _regulatoryTask;

    public BoilerControlService(ILogger<BoilerControlService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _regulatoryTask = new BoilerControlTask(_logger,TimeSpan.FromMilliseconds(5000), _serviceScopeFactory);
        _regulatoryTask.Start();
        _logger.LogInformation("BoilerControlService: Hello from the other side!");
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BoilerControlService stopped");
        await _regulatoryTask.Stop();
    }
}
using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.Enums;

namespace MyHomeAutomation.WebApi;

public class InterrogationTask : PeriodTaskBase
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public InterrogationTask(ILogger logger, TimeSpan interval,
        IServiceScopeFactory serviceScopeFactory) : base(logger, interval)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWhatYouNeed()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
        var relayService = scope.ServiceProvider.GetRequiredService<IRelayService>();

        var relays = await myHomeAutomationDbContext.Relays.Where(r => r.Type == RelayType.Tasmota).ToListAsync().ConfigureAwait(false);

        foreach (var relay in relays)
        {
            try
            {
                var actualRelay = await relayService.GetRelayStatus(relay.Ip).ConfigureAwait(false);

                await relayService.SetValue(relay!.Ip, actualRelay.Status.Power.Equals("1"), RelayType.Tasmota).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { }
            catch (HttpRequestException) { }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
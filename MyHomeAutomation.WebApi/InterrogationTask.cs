namespace MyHomeAutomation.WebApi;

public class InterrogationTask : PeriodTaskBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public InterrogationTask(TimeSpan interval, IServiceScopeFactory serviceScopeFactory) : base(interval)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWhatYouNeed()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
        var relayService = scope.ServiceProvider.GetRequiredService<IRelayService>();

        var relays = myHomeAutomationDbContext.Relays.Where(r => r.Type == 2).ToList();

        foreach (var relay in relays)
        {
            try
            {
                var actualRelay = await relayService.GetRelayStatus(relay.Ip).ConfigureAwait(false);
            
                await relayService.SetValue(relay!.Ip, actualRelay.Status.Power.Equals("1"), 2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
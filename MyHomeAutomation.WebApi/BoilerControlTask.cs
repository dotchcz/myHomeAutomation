using Microsoft.EntityFrameworkCore;

namespace MyHomeAutomation.WebApi;

public class BoilerControlTask : PeriodTaskBase
{
    private const int SoCMacCapacity = 80;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BoilerControlTask(ILogger logger, TimeSpan interval, IServiceScopeFactory serviceScopeFactory) 
        : base(logger, interval)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWhatYouNeed()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
        var relayService = scope.ServiceProvider.GetRequiredService<IRelayService>();
        var inputRegisterService = scope.ServiceProvider.GetRequiredService<IInputRegisterService>();
        var relay = await myHomeAutomationDbContext.Relays.FirstAsync(r => r.Name.Equals("relay:boiler")).ConfigureAwait(false);
        
        var soc = await inputRegisterService.GetSoc();
        
        if (soc.ActualCapacity >= SoCMacCapacity)
        {
            await relayService.SetValue(relay.Ip, true, 2);
        }
        else
        {
            await Task.Delay(TimeSpan.FromMinutes(10));
            soc = await inputRegisterService.GetSoc();
            if (soc.ActualCapacity < SoCMacCapacity)
            {
                await relayService.SetValue(relay.Ip, false, 2); 
            }
        }
    }
}
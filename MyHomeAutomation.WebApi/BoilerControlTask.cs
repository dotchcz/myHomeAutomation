using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.Enums;

namespace MyHomeAutomation.WebApi;

public class BoilerControlTask : PeriodTaskBase
{
    private const double SoCMacCapacity = 50;
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
            await relayService.SetValue(relay.Ip, true, RelayType.Tasmota);
        }
        else
        {
            await Task.Delay(TimeSpan.FromMinutes(10));
            soc = await inputRegisterService.GetSoc();
            if (soc.ActualCapacity < SoCMacCapacity  - 10d)
            {
                await relayService.SetValue(relay.Ip, false, RelayType.Tasmota); 
            }
            else
            {
                await relayService.SetValue(relay.Ip, true, RelayType.Tasmota);
            }
        }
    }
}
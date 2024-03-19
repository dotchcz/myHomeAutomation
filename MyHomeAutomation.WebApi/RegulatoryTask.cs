using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.Enums;

namespace MyHomeAutomation.WebApi;

public class RegulatoryTask : PeriodTaskBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RegulatoryTask(ILogger logger, TimeSpan interval, IServiceScopeFactory serviceScopeFactory)
        : base(logger, interval)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWhatYouNeed()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
        var relayService = scope.ServiceProvider.GetRequiredService<IRelayService>();

        bool pumpInRun;


        var currTempAccuBack = await myHomeAutomationDbContext.Temperatures
            .OrderByDescending(t => t.Created)
            .FirstAsync(t => t.Sensor.Name.Equals("temp:zpatecka")).ConfigureAwait(false);


        // if the temperature (back from accumulation) is higher than 29deg => turn the pump on
        if (currTempAccuBack.Value >= 29)
        {
            // run the pump
            pumpInRun = true;
        }
        else
        {
            var currTempFloor = await myHomeAutomationDbContext.Temperatures
                    .OrderByDescending(t => t.Created)
                    .FirstAsync(t => t.Sensor.Name.Equals("temp:podlahovka")).ConfigureAwait(false)
                ;

            // if the temperature (output accumulation) is lower than 29deg => turn the pump off
            if (currTempFloor?.Value >= 29)
            {
                pumpInRun = true;
            }
            else
            {
                pumpInRun = false;
            }
        }

        var relay = await myHomeAutomationDbContext.Relays.FirstAsync(r => r.Name.Equals("relay:pump"))
            .ConfigureAwait(false);
        await relayService.SetValue(relay!.Ip, pumpInRun, RelayType.Tasmota);
    }
}
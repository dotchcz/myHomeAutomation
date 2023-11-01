namespace MyHomeAutomation.WebApi;

public class RegulatoryTask : PeriodTaskBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RegulatoryTask(TimeSpan interval, IServiceScopeFactory serviceScopeFactory) : base(interval)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWhatYouNeed()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
        var relayService = scope.ServiceProvider.GetRequiredService<IRelayService>();

        // read data from sensory
        var currTempSource = myHomeAutomationDbContext.Temperatures
            .Where(t => t.Sensor.Name.Equals("temp:horka")).ToList()
            .MaxBy(t => t.Created);

        bool pumpInRun;
        if (currTempSource is null || currTempSource.Value < 50)
        {
            pumpInRun = false;
        }
        else
        {
            var currTempAccuBack = myHomeAutomationDbContext.Temperatures
                .Where(t => t.Sensor.Name.Equals("temp:zpatecka")).ToList()
                .MaxBy(t => t.Created);


            // if the temperature (back from accumulation) is higher than 35deg => turn the pump on
            if (currTempAccuBack.Value >= 33)
            {
                // run the pump
                pumpInRun = true;
            }
            else
            {
                var currTempFloor = myHomeAutomationDbContext.Temperatures
                    .Where(t => t.Sensor.Name.Equals("temp:podlahovka")).ToList()
                    .MaxBy(t => t.Created);

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
        }

        var relay = myHomeAutomationDbContext.Relays.First(r => r.Name.Equals("relay:pump"));
        await relayService.SetValue(relay!.Ip, pumpInRun, 2);
    }
}
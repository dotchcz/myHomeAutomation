using Microsoft.EntityFrameworkCore;

namespace MyHomeAutomation.WebApi;

public class RegulatoryTask
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private  Task? _timerTask;
    private readonly PeriodicTimer _periodicTimer;
    private readonly CancellationTokenSource _cts = new();

    public RegulatoryTask(TimeSpan interval, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _periodicTimer = new PeriodicTimer(interval);
    }

    public void Start()
    {
        _timerTask = DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        try
        {
            while (await _periodicTimer.WaitForNextTickAsync(_cts.Token))
            {
                await RegulationAction();
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
    
    private async Task RegulationAction()
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
            if (currTempAccuBack.Value >= 35)
            {
                // run the pump
                pumpInRun = true;
            }
            else
            {
                var currTempFloor = myHomeAutomationDbContext.Temperatures
                    .Where(t => t.Sensor.Name.Equals("temp:podlahovka")).ToList()
                    .MaxBy(t => t.Created);
                
                // if the temperature (output accumulation) is lower than 30deg => turn the pump off
                if (currTempFloor?.Value >= 28)
                {
                    pumpInRun = true;
                }
                else
                {
                    pumpInRun = false;
                }
            }
        }
        
        var relay = myHomeAutomationDbContext.Relays.First(r=>r.Name.Equals("relay:pump"));
        await relayService.SetValue(relay!.Ip, pumpInRun, 2);
    }

    public async Task Stop()
    {
        if (_timerTask is null)
        {
            return;
        }
        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
        Console.WriteLine("RegulatoryTask stopped");
    }
}
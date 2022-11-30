using Microsoft.EntityFrameworkCore;

namespace MyHomeAutomation.WebApi;

public class RegulatoryService: IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RegulatoryService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
               var myHomeAutomationDbContext = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
                
               // read data from sensory
               var currTempAccu = await myHomeAutomationDbContext.Temperatures
                   .Where(t=>t.Sensor.Name.Equals("temp:zpatecka"))
                   .MaxAsync(t => t.Created, cancellationToken: cancellationToken);
            
               var currTempFloor = await myHomeAutomationDbContext.Temperatures
                   .Where(t=>t.Sensor.Name.Equals("temp:podlahovka"))
                   .MaxAsync(t => t.Created, cancellationToken: cancellationToken);
            
               // if the temperature (input accumulation) is higher than 45deg => turn the pump on
               // if the temperature (output accumulation) is lower than 30def => turn the pump off
               
               Thread.Sleep(5000);
            }
        }
        
        
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Service stopped");
        return Task.CompletedTask;
    }
}


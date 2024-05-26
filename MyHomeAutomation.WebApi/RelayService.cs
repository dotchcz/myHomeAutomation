using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.ApiModels;
using MyHomeAutomation.WebApi.Enums;
using Newtonsoft.Json;

namespace MyHomeAutomation.WebApi;

public class RelayService : IRelayService
{
    private readonly MyHomeAutomationDbContext _dbContext;
    private readonly ILogger<RelayService> _logger;
    private readonly HttpClient _httpClient;

    private readonly Func<string, bool, RelayType, Uri> _urlMappingSet = (ip, active, type) =>
    {
        return type switch
        {
            RelayType.Wemos => new Uri($"http://{ip}/LED=" + (active ? "ON" : "OFF")),
            RelayType.Tasmota => new Uri($"http://{ip}/cm?cmnd=Power%20" + (active ? "On" : "Off")),
            _ => throw new ArgumentException($"RelayService: Unknown type value {type}")
        };
    };
    
    private readonly Func<string, RelayType, Uri> _urlMappingGet = (ip, type) =>
    {
        return type switch
        {
            RelayType.Wemos => new Uri($"http://{ip}"),
            RelayType.Tasmota => new Uri($"http://{ip}/cm?cmnd=status%200"),
            _ => throw new ArgumentException($"RelayService: Unknown type value {type}")
        };
    };

    public RelayService(MyHomeAutomationDbContext dbContext, ILogger<RelayService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("myHomeAutomationClient");
    }

    public async Task SetValue(string ip, bool active, RelayType type)
    {
        try
        {
            var uri = _urlMappingSet(ip, active, type);
            await _httpClient.GetAsync(uri).ConfigureAwait(false);

            var relay = await _dbContext.Relays.FirstAsync(r => r.Ip.Equals(ip));
            relay.Active = active;
            relay.LastUpdate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
        catch (TaskCanceledException)
        {
        }
        catch (HttpRequestException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
    
    public async Task SetValueExtending(RelayRequest request)
    {
        try
        {
            var relay = await _dbContext.Relays.FindAsync(request.Id);

            var uri = _urlMappingSet(relay!.Ip, request.Active, relay.Type);

            await _httpClient.GetAsync(uri);

            relay!.Active = request.Active;
            relay.LastUpdate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            if (request.Active)
            {
                var timeSpan = relay!.Delay ?? TimeSpan.FromMinutes(5);
                await Task.Delay(timeSpan);
                uri = _urlMappingSet(relay!.Ip, !request.Active, relay.Type);
                await _httpClient.GetAsync(uri);

                relay!.Active = !request.Active;
                relay.LastUpdate = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<T> GetRelayStatus<T>(string ip, RelayType relayType)
    {
        var uri = _urlMappingGet(ip, relayType);
        var res = await _httpClient.GetAsync(uri);
        string responseBody = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseBody)!;
    }
}

public interface IRelayService
{
    Task SetValue(string ip, bool active, RelayType type);
    Task SetValueExtending(RelayRequest request);
    Task<T> GetRelayStatus<T>(string ip, RelayType relayType);
}
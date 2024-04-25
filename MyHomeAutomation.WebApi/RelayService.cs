using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.ApiModels;
using MyHomeAutomation.WebApi.Dto;
using MyHomeAutomation.WebApi.Enums;
using Newtonsoft.Json;
using Polly;

namespace MyHomeAutomation.WebApi;

public class RelayService : IRelayService
{
    private readonly MyHomeAutomationDbContext _dbContext;
    private readonly ILogger<RelayService> _logger;
    private readonly HttpClient _httpClient;
    
    private readonly Func<string, bool, RelayType, Uri> _urlMapping = (ip, active, type) =>
    {
        return type switch
        {
            RelayType.Wemos => new Uri($"http://{ip}/LED=" + (active ? "ON" : "OFF")),
            RelayType.Tasmota => new Uri($"http://{ip}/cm?cmnd=Power%20" + (active ? "On" : "Off")),
            _ => throw new ArgumentException($"RelayService: Unknown type value {type}")
        };
    };

    public RelayService(MyHomeAutomationDbContext dbContext, ILogger<RelayService> logger, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("myHomeAutomationClient");
    }

    public async Task SetValue(string ip, bool active, RelayType type)
    {
        try
        {
            var uri = _urlMapping(ip, active, type);
            await _httpClient.GetAsync(uri).ConfigureAwait(false);
            
            var relay = await _dbContext.Relays.FirstAsync(r => r.Ip.Equals(ip)).ConfigureAwait(false);
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

            var uri = _urlMapping(relay!.Ip, request.Active, relay.Type);

            await _httpClient.GetAsync(uri).ConfigureAwait(false);

            relay!.Active = request.Active;
            relay.LastUpdate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            if (request.Active)
            {
                var timeSpan = relay!.Delay ?? TimeSpan.FromMinutes(5);
                await Task.Delay(timeSpan);
                uri = _urlMapping(relay!.Ip, !request.Active, relay.Type);
                await _httpClient.GetAsync(uri).ConfigureAwait(false);

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

    public async Task<TasmotaRelayDto> GetRelayStatus(string ip)
    {
        var uri = new Uri($"http://{ip}/cm?cmnd=status%200");
        var res = await _httpClient.GetAsync(uri).ConfigureAwait(false);

        string responseBody = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonConvert.DeserializeObject<TasmotaRelayDto>(responseBody)!;
    }
}

public interface IRelayService
{
    Task SetValue(string ip, bool active, RelayType type);
    Task SetValueExtending(RelayRequest request);
    Task<TasmotaRelayDto> GetRelayStatus(string ip);
}
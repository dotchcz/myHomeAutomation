using MyHomeAutomation.WebApi.Dto;
using MyHomeAutomation.WebApi.Models;
using Newtonsoft.Json;

namespace MyHomeAutomation.WebApi;


public class RelayService : IRelayService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly Func<string, bool, int, Uri> _urlMapping = (ip, active, type) =>
    {
        if (type == 1)
        {
            return new Uri($"http://{ip}/LED=" + (active ? "ON" : "OFF"));
        }
        if (type == 2)
        {
            return new Uri($"http://{ip}/cm?cmnd=Power%20" + (active ? "On" : "Off"));
        }

        throw new ArgumentException($"RelayService: Unknown type value {type}");
    };

    public RelayService(HttpClient httpClient, IServiceScopeFactory serviceScopeFactory)
    {
        _httpClient = httpClient;
        _serviceScopeFactory = serviceScopeFactory;
        _httpClient.Timeout = new TimeSpan(0,0,2);
    }

    public async Task SetValue(string ip, bool active, int type)
    {
        try
        {
            var uri = _urlMapping(ip, active, type);
            var res = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyHomeAutomationDbContext>();
            var relay = context.Relays.First(r=>r.Ip.Equals(ip));
            relay.Active = active;
            relay.LastUpdate = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
    Task SetValue(string ip, bool active, int type);
    Task<TasmotaRelayDto> GetRelayStatus(string ip);
}
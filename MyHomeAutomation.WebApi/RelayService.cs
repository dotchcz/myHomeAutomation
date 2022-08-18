using MyHomeAutomation.WebApi.Dto;
using MyHomeAutomation.WebApi.Models;

namespace MyHomeAutomation.WebApi;


public class RelayService : IRelayService
{
    private readonly HttpClient _httpClient;

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

        throw new ArgumentException($"Unknown type value {type}");
    };

    public RelayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = new TimeSpan(0,5,0);
    }

    public async Task SetValue(string ip, bool active, int type)
    {

        var uri = _urlMapping(ip, active, type);
        var res = await _httpClient.GetAsync(uri);
    }
}

public interface IRelayService
{
    Task SetValue(string ip, bool active, int type);
}
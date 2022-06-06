using MyHomeAutomation.WebApi.Dto;
using MyHomeAutomation.WebApi.Models;

namespace MyHomeAutomation.WebApi;

public class RelayService : IRelayService
{
    private readonly HttpClient _httpClient;

    public RelayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SetValue(string ip, bool active)
    {
        var uri = new Uri($"http://{ip}/LED=" + (active ? "ON" : "OFF"));
        var res = await _httpClient.GetAsync(uri);
    }
}

public interface IRelayService
{
    Task SetValue(string ip, bool active);
}
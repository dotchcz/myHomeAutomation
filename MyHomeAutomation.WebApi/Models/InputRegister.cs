namespace MyHomeAutomation.WebApi.Models;

public class InputRegister
{
    public int Id { get; set; }
    public int RegisterId { get; set; }
    public int Value { get; set; }
    public DateTimeOffset LastUpdate { get; set; }
}
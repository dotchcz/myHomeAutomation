namespace MyHomeAutomation.WebApi.ApiModels;

public class SensorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public DateTime Created { get; set; }
}
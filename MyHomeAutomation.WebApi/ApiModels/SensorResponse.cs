namespace MyHomeAutomation.WebApi.ApiModels;

public class SensorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal? ValueTemp { get; set; }
    public decimal? ValueHumidity { get; set; }
    public DateTime? CreatedTemp { get; set; }
    public DateTime? CreatedHumidity { get; set; }
}
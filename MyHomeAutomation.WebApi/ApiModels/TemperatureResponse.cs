namespace MinimalApi.ApiModels;

public class TemperatureResponse
{
    // select loc.name as locationName, temp.value, sens.name as sensorName, temp.created
    public string LocationName { get; set; }
    public decimal Value { get; set; }
    public string SensorName { get; set; }
    public DateTime Created { get; set; }
}
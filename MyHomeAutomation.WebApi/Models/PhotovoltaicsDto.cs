namespace MyHomeAutomation.WebApi.Models;

public class PhotovoltaicsDto
{
    // PV
    public float SolarEnergyToday { get; set; }
    public float SolarEnergyTotal { get; set; }
    public float PvVoltage1 { get; set; }
    public float PvVoltage2 { get; set; }
    public float PvCurrent1 { get; set; }
    public float PvCurrent2 { get; set; }
    public float PowerDc1 { get; set; }
    public float PowerDc2 { get; set; }
    
    // battery
    public float BatVoltageCharge { get; set; }
    public float BatCurrentCharge { get; set; }
    public float BatPowerCharge { get; set; }
    public float TemperatureBat { get; set; }
    public float BatteryCapacity { get; set; }
    public bool BmsConnectState { get; set; }
    public float OutputEnergyCharge { get; set; }
    public float InputEnergyCharge { get; set; }
    public float OutputEnergyChargeToday { get; set; }
    public float InputEnergyChargeToday { get; set; }
    
    // inverter
    public float GridVoltage { get; set; }
    public float GridCurrent { get; set; }
    public float GridPower { get; set; }
    public float TemperatureInverter { get; set; }
    public float FeedInPower { get; set; }
    public float FeedInEnergyTotal { get; set; }
    public float ConsumeEnergyTotal { get; set; }
    public float OffGridPower { get; set; }
    public string RunMode { get; set; }

    // EV
    public float PowerToEv { get; set; }
    
    // General
    public DateTime LastUpdate { get; set; }
}
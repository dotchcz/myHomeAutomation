using Microsoft.EntityFrameworkCore;
using MyHomeAutomation.WebApi.ApiModels;
using MyHomeAutomation.WebApi.Dto;
using MyHomeAutomation.WebApi.Models;

namespace MyHomeAutomation.WebApi;

public interface IInputRegisterService
{
    Task Update(int offset, ReadInputRegisterRequest readInputRegister);
    Task<SocDto> GetSoc();
    Task<PhotovoltaicsDto> GetPhotovoltaics();
}

public enum InputRegisterEnum
{
    GridVoltage = 0x0,
    GridCurrent = 0x1,
    GridPower = 0x2,
    PvVoltage1 = 0x0003,
    PvVoltage2 = 0x0004,
    PvCurrent1 = 0x0005,
    PvCurrent2 = 0x0006,
    Powerdc1 = 0x000A,
    Powerdc2 = 0x000B,

    TemperatureInverter = 0x8,
    RunMode = 0x9,
    BatVoltage_Charge1 = 0x14,
    BatCurrent_Charge1 = 0x15,
    Batpower_Charge1 = 0x0016,
    BMS_Connect_State = 0x0017,
    TemperatureBat = 0x0018,
    Battery_Capacity = 0x001C,
    PowerToEVLSB = 0x26,
    PowerToEVMSB = 0x27,
    Feedin_powerLSB = 0x46,
    Feedin_powerMSB = 0x47,
    Feedin_energy_totalLSB = 0x48,
    Feedin_energy_totalMSB = 0x49,
    Consum_energy_totalLSB =   0x004A ,
    Consum_energy_totalMSB = 0x004B,
    Off_gridPower = 0x004E,
    SolarEnergyToday = 0x0096,
    SolarEnergyTotalLsb = 0x0094,
    SolarEnergyTotalMsb = 0x0095,
    TargetSoc = 0x011B,
    SocUpper = 0x011C,
    SocLower = 0x011D,
    OutputEnergy_ChargeLSB = 0x001D,
    OutputEnergy_ChargeMSB = 0x001E,
    OutputEnergy_Charge_today = 0x0020,
    InputEnergy_ChargeLSB = 0x0021,
    InputEnergy_ChargeMSB = 0x0022,
    InputEnergy_Charge_today = 0x0023
}

public enum RunModeDescription
{
    Waiting,
    Checking,
    Normal,
    Fault,
    PermanentFault,
    Update,
    OffGridWaiting,
    OffGrid,
    SelfTesting,
    Idle,
    Standby
}

public record InputRegisterInfo(InputRegisterEnum[] InputRegisterEnum, double Unit, string Description, double Value);

public class InputRegisterService : IInputRegisterService
{
    private readonly MyHomeAutomationDbContext _dbContext;

    private readonly IDictionary<InputRegisterEnum, float> _inputRegisterValueMapBaterry =
        new Dictionary<InputRegisterEnum, float>()
        {
            {InputRegisterEnum.TargetSoc, 1},
            {InputRegisterEnum.SocUpper, 1},
            {InputRegisterEnum.SocLower, 1},
            {InputRegisterEnum.Battery_Capacity, 1},
        };

    private readonly IDictionary<InputRegisterEnum, float> _inputRegisterValueMapPhotovoltaics =
        new Dictionary<InputRegisterEnum, float>()
        {
            {InputRegisterEnum.GridVoltage, .1f},
            {InputRegisterEnum.GridCurrent, .1f},
            {InputRegisterEnum.GridPower, 1},
            {InputRegisterEnum.PvVoltage1, .1f},
            {InputRegisterEnum.PvVoltage2, .1f},
            {InputRegisterEnum.PvCurrent1, .1f},
            {InputRegisterEnum.PvCurrent2, .1f},
            {InputRegisterEnum.TemperatureInverter, 1},
            {InputRegisterEnum.Powerdc1, 1},
            {InputRegisterEnum.Powerdc2, 1},
            {InputRegisterEnum.BatVoltage_Charge1, .1f},
            {InputRegisterEnum.BatCurrent_Charge1, .1f},
            {InputRegisterEnum.Batpower_Charge1, 1},
            {InputRegisterEnum.BMS_Connect_State, 1}, // bool
            {InputRegisterEnum.TemperatureBat, 1},
            {InputRegisterEnum.Battery_Capacity, 1},
            {InputRegisterEnum.PowerToEVLSB, 1},
            {InputRegisterEnum.PowerToEVMSB, 1},
            {InputRegisterEnum.Feedin_powerLSB, 1},
            {InputRegisterEnum.Feedin_powerMSB, 1},
            {InputRegisterEnum.Feedin_energy_totalLSB, .01f},
            {InputRegisterEnum.Feedin_energy_totalMSB, .01f},
            {InputRegisterEnum.Consum_energy_totalLSB, .01f},
            {InputRegisterEnum.Consum_energy_totalMSB, .01f},
            {InputRegisterEnum.Off_gridPower, 1},
            {InputRegisterEnum.SolarEnergyToday, .1f},
            {InputRegisterEnum.SolarEnergyTotalLsb, .1f},
            {InputRegisterEnum.SolarEnergyTotalMsb, .1f},
            {InputRegisterEnum.RunMode, 1}, // enum
            {InputRegisterEnum.OutputEnergy_ChargeLSB, .1f},
            {InputRegisterEnum.OutputEnergy_ChargeMSB, .1f},
            {InputRegisterEnum.OutputEnergy_Charge_today, .1f},
            {InputRegisterEnum.InputEnergy_ChargeLSB, .1f},
            {InputRegisterEnum.InputEnergy_ChargeMSB, .1f},
            {InputRegisterEnum.InputEnergy_Charge_today, .1f},
        };

    public InputRegisterService(MyHomeAutomationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Update(int offset, ReadInputRegisterRequest readInputRegister)
    {
        var all = await _dbContext.InputRegisters
            .OrderBy(i => i.Id).Skip(offset).Take(readInputRegister.Values.Length).ToListAsync();

        var lastUpdate = DateTimeOffset.UtcNow;

        var index = 0;
        foreach (var inputRegister in all)
        {
            inputRegister.RegisterId = index + offset;
            inputRegister.Value = readInputRegister.Values[index];
            inputRegister.LastUpdate = lastUpdate;
            index++;
        }

        if (!all.Any())
        {
            for (int i = 0; i < readInputRegister.Values.Length; i++)
            {
                var entity = new InputRegister()
                {
                    RegisterId = i + offset,
                    Value = readInputRegister.Values[i],
                    LastUpdate = lastUpdate
                };
                await _dbContext.InputRegisters.AddAsync(entity);
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<SocDto> GetSoc()
    {
        var rawSoc = await _dbContext.InputRegisters
            .FirstAsync(i => i.RegisterId == (int) InputRegisterEnum.Battery_Capacity);

        return new SocDto()
            {ActualCapacity = rawSoc.Value * _inputRegisterValueMapBaterry[InputRegisterEnum.Battery_Capacity]};
    }

    public async Task<PhotovoltaicsDto> GetPhotovoltaics()
    {
        var raw = await _dbContext.InputRegisters
            .Where(i => _inputRegisterValueMapPhotovoltaics.Keys
                .Select(k => (int) k).Contains(i.RegisterId))
            .ToListAsync();

        var result = new PhotovoltaicsDto()
        {
            LastUpdate = raw.FirstOrDefault() is null
                ? DateTime.MinValue
                : raw.First().LastUpdate.DateTime.ToLocalTime()
        };
        foreach (var inputRegister in raw)
        {
            switch (inputRegister.RegisterId)
            {
                case (int) InputRegisterEnum.GridVoltage:
                    result.GridVoltage = GetValue(raw, InputRegisterEnum.GridVoltage);
                    break;
                case (int) InputRegisterEnum.GridCurrent:
                    result.GridCurrent = GetValue(raw, InputRegisterEnum.GridCurrent);
                    break;
                case (int) InputRegisterEnum.GridPower:
                    result.GridPower = GetValue(raw, InputRegisterEnum.GridPower);
                    break;
                case (int) InputRegisterEnum.PvVoltage1:
                    result.PvVoltage1 = GetValue(raw, InputRegisterEnum.PvVoltage1);
                    break;
                case (int) InputRegisterEnum.PvVoltage2:
                    result.PvVoltage2 = GetValue(raw, InputRegisterEnum.PvVoltage2);
                    break;
                case (int) InputRegisterEnum.PvCurrent1:
                    result.PvCurrent1 = GetValue(raw, InputRegisterEnum.PvCurrent1);
                    break;
                case (int) InputRegisterEnum.PvCurrent2:
                    result.PvCurrent2 = GetValue(raw, InputRegisterEnum.PvCurrent2);
                    break;
                case (int) InputRegisterEnum.TemperatureInverter:
                    result.TemperatureInverter = GetValue(raw, InputRegisterEnum.TemperatureInverter);
                    break;
                case (int) InputRegisterEnum.Powerdc1:
                    result.PowerDc1 = GetValue(raw, InputRegisterEnum.Powerdc1);
                    break;
                case (int) InputRegisterEnum.Powerdc2:
                    result.PowerDc2 = GetValue(raw, InputRegisterEnum.Powerdc2);
                    break;
                case (int) InputRegisterEnum.BatVoltage_Charge1:
                    result.BatVoltageCharge = GetValue(raw, InputRegisterEnum.BatVoltage_Charge1);
                    break;
                case (int) InputRegisterEnum.BatCurrent_Charge1:
                    result.BatCurrentCharge = GetValue(raw, InputRegisterEnum.BatCurrent_Charge1);
                    break;
                case (int) InputRegisterEnum.Batpower_Charge1:
                    result.BatPowerCharge = GetValue(raw, InputRegisterEnum.Batpower_Charge1);
                    break;
                case (int) InputRegisterEnum.BMS_Connect_State:
                    result.BmsConnectState = GetBoolValue(raw, InputRegisterEnum.BMS_Connect_State);
                    break;
                case (int) InputRegisterEnum.TemperatureBat:
                    result.TemperatureBat = GetValue(raw, InputRegisterEnum.TemperatureBat);
                    break;
                case (int) InputRegisterEnum.Battery_Capacity:
                    result.BatteryCapacity = GetValue(raw, InputRegisterEnum.Battery_Capacity);
                    break;
                case (int) InputRegisterEnum.PowerToEVLSB:
                case (int) InputRegisterEnum.PowerToEVMSB:
                    result.PowerToEv =
                        GetValue(raw, InputRegisterEnum.PowerToEVLSB) + GetValue(raw, InputRegisterEnum.PowerToEVMSB);
                    break;
                case (int) InputRegisterEnum.Feedin_powerLSB:
                case (int) InputRegisterEnum.Feedin_powerMSB:
                    result.FeedInPower = GetCombinedValue(raw, InputRegisterEnum.Feedin_powerLSB,
                        InputRegisterEnum.Feedin_powerMSB);
                    break;
                case (int) InputRegisterEnum.Feedin_energy_totalLSB:
                case (int) InputRegisterEnum.Feedin_energy_totalMSB:
                    result.FeedInEnergyTotal = GetCombinedValue(raw, InputRegisterEnum.Feedin_energy_totalLSB,
                        InputRegisterEnum.Feedin_energy_totalMSB);
                    break;
                case (int) InputRegisterEnum.Consum_energy_totalLSB:
                case (int) InputRegisterEnum.Consum_energy_totalMSB:
                    result.ConsumeEnergyTotal = GetCombinedValue(raw, InputRegisterEnum.Consum_energy_totalLSB,
                        InputRegisterEnum.Consum_energy_totalMSB);
                    break;
                case (int) InputRegisterEnum.Off_gridPower:
                    result.OffGridPower = GetValue(raw, InputRegisterEnum.Off_gridPower);
                    break;
                case (int) InputRegisterEnum.SolarEnergyToday:
                    result.SolarEnergyToday = GetValue(raw, InputRegisterEnum.SolarEnergyToday);
                    break;
                case (int) InputRegisterEnum.SolarEnergyTotalLsb:
                case (int) InputRegisterEnum.SolarEnergyTotalMsb:
                    result.SolarEnergyTotal = GetCombinedValue(raw, InputRegisterEnum.SolarEnergyTotalLsb,
                        InputRegisterEnum.SolarEnergyTotalMsb);
                    break;
                case (int) InputRegisterEnum.RunMode:
                    result.RunMode = GetEnumValue(raw, InputRegisterEnum.RunMode, typeof(RunModeDescription));
                    break;
                case (int) InputRegisterEnum.InputEnergy_Charge_today:
                    result.InputEnergyChargeToday = GetValue(raw, InputRegisterEnum.InputEnergy_Charge_today);
                    break;
                case (int) InputRegisterEnum.OutputEnergy_Charge_today:
                    result.OutputEnergyChargeToday = GetValue(raw, InputRegisterEnum.OutputEnergy_Charge_today);
                    break;
                case (int) InputRegisterEnum.InputEnergy_ChargeLSB:
                case (int) InputRegisterEnum.InputEnergy_ChargeMSB:
                    result.InputEnergyCharge = GetCombinedValue(raw, InputRegisterEnum.InputEnergy_ChargeLSB,
                        InputRegisterEnum.InputEnergy_ChargeMSB);
                    break;
                case (int) InputRegisterEnum.OutputEnergy_ChargeLSB:
                case (int) InputRegisterEnum.OutputEnergy_ChargeMSB:
                    result.OutputEnergyCharge = GetCombinedValue(raw, InputRegisterEnum.OutputEnergy_ChargeLSB,
                        InputRegisterEnum.OutputEnergy_ChargeMSB);
                    break;
            }
        }

        return result;
    }

    private float GetCombinedValue(IList<InputRegister> raw, InputRegisterEnum enumLsb, InputRegisterEnum enumMsb)
    {
        var msb = (ushort) raw.First(f => f.RegisterId == (int) enumMsb).Value;
        var lsb = (ushort) raw.First(f => f.RegisterId == (int) enumLsb).Value;

        var combinedValue = ((uint) msb << 16) | lsb;
        var ret = combinedValue * _inputRegisterValueMapPhotovoltaics[@enumLsb];
        return ret;
    }


    private float GetValue(IList<InputRegister> raw, InputRegisterEnum @enum)
    {
        return (raw.First(f => f.RegisterId == (int) @enum).Value *
                _inputRegisterValueMapPhotovoltaics[@enum]);
    }

    private bool GetBoolValue(IList<InputRegister> raw, InputRegisterEnum @enum)
    {
        return raw.First(f => f.RegisterId == (int) @enum).Value == 1;
    }

    private string GetEnumValue(IList<InputRegister> raw, InputRegisterEnum @enum, Type enumType)
    {
        var value = raw.First(f => f.RegisterId == (int) @enum).Value;
        return Enum.GetName(enumType, value);
    }
}
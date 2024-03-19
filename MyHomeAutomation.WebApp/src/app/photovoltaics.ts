export class Photovoltaics {
    // PV
    solarEnergyToday: number;
    solarEnergyTotal: number;
    pvVoltage1: number;
    pvVoltage2: number;
    pvCurrent1: number;
    pvCurrent2: number;
    powerDc1: number;
    powerDc2: number;
  
    // battery
    batVoltageCharge: number;
    batCurrentCharge: number;
    batPowerCharge: number;
    temperatureBat: number;
    batteryCapacity: number;
    bmsConnectState: boolean;
  
    // inverter
    runModeDescription: string;
    gridVoltage: number;
    gridCurrent: number;
    gridPower: number;
    temperatureInverter: number;
    feedInPower: number;
    feedInEnergyTotal: number;
    offGridPower: number;
    runMode: string;
  
    // EV
    powerToEv: number;

    lastUpdate: Date;
  
    constructor() {
      this.solarEnergyToday = 0;
      this.solarEnergyTotal = 0;
      this.pvVoltage1 = 0;
      this.pvVoltage2 = 0;
      this.pvCurrent1 = 0;
      this.pvCurrent2 = 0;
      this.powerDc1 = 0;
      this.powerDc2 = 0;
      this.batVoltageCharge = 0;
      this.batCurrentCharge = 0;
      this.batPowerCharge = 0;
      this.temperatureBat = 0;
      this.batteryCapacity = 0;
      this.bmsConnectState = false;
      this.runModeDescription = '';
      this.gridVoltage = 0;
      this.gridCurrent = 0;
      this.gridPower = 0;
      this.temperatureInverter = 0;
      this.feedInPower = 0;
      this.feedInEnergyTotal = 0;
      this.offGridPower = 0;
      this.runMode = '';
      this.powerToEv = 0;
      this.lastUpdate = new Date()
    }
  }
  
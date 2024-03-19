import { Component, OnInit } from '@angular/core';
import {Photovoltaics} from '../photovoltaics';
import { Relay } from '../relay';
import {PhotovoltaicsService} from '../photovoltaics.service';
import {CommonService} from "../common.service";

@Component({
  selector: 'app-photovoltaics',
  templateUrl: './photovoltaics.component.html',
  styleUrls: ['./photovoltaics.component.css']
})
export class PhotovoltaicsComponent implements OnInit {
  photovoltaics: Photovoltaics | null = null;
  interval: any

  pvDictionary: Array<{ key: string, value: any }> = [];
  batteryDictionary: Array<{ key: string, value: any }> = [];
  gridDictionary: Array<{ key: string, value: any }> = [];
  evDictionary: Array<{ key: string, value: any }> = [];

  constructor(private photovoltaicsService: PhotovoltaicsService, public commonService: CommonService) {
  }

  ngOnInit(): void {
    this.getPhotovoltaics();
    this.interval = setInterval(() => {
        this.getPhotovoltaics();
    }, 2000);
  }

    getPhotovoltaics(): void {
      this.photovoltaicsService.getPhotovoltaics()
          .subscribe(photovoltaics => {
            this.pvDictionary = [];
            this.batteryDictionary = [];
            this.gridDictionary = [];
            this.evDictionary = [];

            this.photovoltaics = photovoltaics;
            
            this.pvDictionary.push({key: 'powerDcTotal [W]', value: photovoltaics.powerDc1 + photovoltaics.powerDc2});
            this.pvDictionary.push({key: 'powerDc1 [W]', value: photovoltaics.powerDc1});
            this.pvDictionary.push({key: 'powerDc2 [W]', value: photovoltaics.powerDc2});
            this.pvDictionary.push({key: 'pvVoltage1 [V]', value: photovoltaics.pvVoltage1});
            this.pvDictionary.push({key: 'pvVoltage2 [V]', value: photovoltaics.pvVoltage2});
            this.pvDictionary.push({key: 'pvCurrent1 [A]', value: photovoltaics.pvCurrent1});
            this.pvDictionary.push({key: 'pvCurrent2 [A]', value: photovoltaics.pvCurrent2});          
            this.pvDictionary.push({key: 'solarEnergyToday [kWh]', value: photovoltaics.solarEnergyToday});
            this.pvDictionary.push({key: 'solarEnergyTotal [MWh]', value: photovoltaics.solarEnergyTotal});

            this.batteryDictionary.push({key: 'batteryCapacity [%]', value: photovoltaics.batteryCapacity});
            this.batteryDictionary.push({key: 'batPowerCharge [W]', value: photovoltaics.batPowerCharge});
            this.batteryDictionary.push({key: 'batCurrentCharge [A]', value: photovoltaics.batCurrentCharge});
            this.batteryDictionary.push({key: 'batVoltageCharge [V]', value: photovoltaics.batVoltageCharge});
            this.batteryDictionary.push({key: 'bmsConnectState', value: photovoltaics.bmsConnectState});
            this.batteryDictionary.push({key: 'temperatureBat [C]', value: photovoltaics.temperatureBat});

            this.gridDictionary.push({key: 'actualCons [W]', value: photovoltaics.gridPower - photovoltaics.feedInPower});
            this.gridDictionary.push({key: 'gridPower [W]', value: photovoltaics.gridPower});
            this.gridDictionary.push({key: 'feedInPower [W]', value: photovoltaics.feedInPower});
            this.gridDictionary.push({key: 'gridVoltage [V]', value: photovoltaics.gridVoltage});
            this.gridDictionary.push({key: 'gridCurrent [A]', value: photovoltaics.gridCurrent});
            this.gridDictionary.push({key: 'tempInv [C]', value: photovoltaics.temperatureInverter});
            this.gridDictionary.push({key: 'feedInEnergyTotal [kWh]', value: photovoltaics.feedInEnergyTotal});
            this.gridDictionary.push({key: 'consumeEnergyTotal [kWh]', value: photovoltaics.consumeEnergyTotal});
            this.gridDictionary.push({key: 'offGridPower [W]', value: photovoltaics.offGridPower});
            this.gridDictionary.push({key: 'runMode', value: photovoltaics.runMode});

            this.evDictionary.push({key: 'powerToEv', value: photovoltaics.powerToEv});
            this.evDictionary.push({key: 'inputEnergyCharge [kWh]', value: photovoltaics.inputEnergyCharge});
            this.evDictionary.push({key: 'outputEnergyCharge [kWh]', value: photovoltaics.outputEnergyCharge});
            this.evDictionary.push({key: 'inputEnergyChargeToday [kWh]', value: photovoltaics.inputEnergyChargeToday});
            this.evDictionary.push({key: 'outputEnergyChargeToday [kWh]', value: photovoltaics.outputEnergyChargeToday});
          });
  }

}

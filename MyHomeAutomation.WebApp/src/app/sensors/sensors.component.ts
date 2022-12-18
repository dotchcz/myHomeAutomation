import { Component, OnInit } from '@angular/core';
import {Location} from "@angular/common";

import {Sensor} from "../sensor";
import {SensoryService} from "../sensory.service";
import {CommonService} from "../common.service";

@Component({
  selector: 'app-sensors',
  templateUrl: './sensors.component.html',
  styleUrls: ['./sensors.component.css']
})
export class SensorsComponent implements OnInit {
  sensors: Sensor[] = [];
  interval: any;
  
  constructor(private location: Location, private sensoryService: SensoryService, public commonService: CommonService) { }

  ngOnInit(): void {
    this.getSensors();
    this.interval = setInterval(() => {
      this.getSensors();
    }, 2000);
  }

  getSensors(): void{
    this.sensoryService.getSensors()
        .subscribe(sensors=> this.sensors = sensors);
  }
  
  goBack(): void {
    this.location.back();
  }
}

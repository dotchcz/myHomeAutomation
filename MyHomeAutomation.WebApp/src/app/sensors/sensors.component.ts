import { Component, OnInit } from '@angular/core';
import {Location} from "@angular/common";

import {Sensor} from "../sensor";
import {SensoryService} from "../sensory.service";

@Component({
  selector: 'app-sensors',
  templateUrl: './sensors.component.html',
  styleUrls: ['./sensors.component.css']
})
export class SensorsComponent implements OnInit {
  sensors: Sensor[] = [];
  
  constructor(private location: Location, private sensoryService: SensoryService) { }

  ngOnInit(): void {
    this.getSensors();
  }

  getSensors(): void{
    this.sensoryService.getSensors()
        .subscribe(sensors=> this.sensors = sensors);
  }
  
  goBack(): void {
    this.location.back();
  }
}

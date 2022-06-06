import { Component, OnInit } from '@angular/core';
import {Location} from "@angular/common";

@Component({
  selector: 'app-sensor-detail',
  templateUrl: './sensor-detail.component.html',
  styleUrls: ['./sensor-detail.component.css']
})
export class SensorDetailComponent implements OnInit {

  constructor(private location: Location) { }

  ngOnInit(): void {
  }
  
  goBack(): void {
    this.location.back();
  }
}

import { Component, OnInit } from '@angular/core';
import {Location} from "@angular/common";

@Component({
  selector: 'app-sensors',
  templateUrl: './sensors.component.html',
  styleUrls: ['./sensors.component.css']
})
export class SensorsComponent implements OnInit {

  constructor(private location: Location) { }

  ngOnInit(): void {
  }
  
  goBack(): void {
    this.location.back();
  }
}

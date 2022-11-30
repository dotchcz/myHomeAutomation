import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MessageService} from "./message.service";
import {Observable} from "rxjs";
import {Sensor} from "./sensor";

@Injectable({
  providedIn: 'root'
})
export class SensoryService {

  private baseURL = `http://178.72.196.140:5266/sensors`
  //private baseURL = `http://localhost:5266/sensors`
  
  constructor(private http: HttpClient, private messageService: MessageService) { }

  getSensors(): Observable<Sensor[]> {
    this.messageService.add('SensoryService: fetched sensors, ' + this.baseURL);
    return this.http.get<Sensor[]>(this.baseURL)
  }
}

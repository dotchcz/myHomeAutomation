import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MessageService} from "./message.service";
import {Observable} from "rxjs";
import {Sensor} from "./sensor";
import { ConfigService } from './config.service'; // Import ConfigService

@Injectable({
  providedIn: 'root'
})
export class SensoryService {

  private baseURL = ``
  
  
  constructor(private http: HttpClient, private messageService: MessageService, private configService: ConfigService) {
    this.baseURL = `${this.configService.apiUrl}/sensors`
  }

  getSensors(): Observable<Sensor[]> {
    this.messageService.add('SensoryService: fetched sensors, ' + this.baseURL);
    return this.http.get<Sensor[]>(this.baseURL)
  }
}

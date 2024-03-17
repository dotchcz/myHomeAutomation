import { Injectable } from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';

import {Photovoltaics} from './photovoltaics';
import {MessageService} from "./message.service";
import { ConfigService } from './config.service'; // Import ConfigService


@Injectable({
  providedIn: 'root'
})
export class PhotovoltaicsService {
  private baseURL = ``
  
  
  constructor(private http: HttpClient, private messageService: MessageService, private configService: ConfigService) {
    this.baseURL = `${this.configService.apiUrl}/photovoltaics`
  }

  getPhotovoltaics(): Observable<Photovoltaics> {
    this.messageService.add('PhotovoltaicsService: fetched photovoltaics, ' + this.baseURL);
    return this.http.get<Photovoltaics>(this.baseURL)
  }
}

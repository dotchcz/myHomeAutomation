import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';

import {Relay} from './relay';
import {MessageService} from "./message.service";
import { ConfigService } from './config.service'; // Import ConfigService

@Injectable({
    providedIn: 'root'
})
export class RelayService {
    
    private baseURL = ``
    private baseURLExtendingRelays = ``

    constructor(private http: HttpClient, private messageService: MessageService, private configService: ConfigService) {
        this.baseURL = `${this.configService.apiUrl}/relays`;
        this.baseURLExtendingRelays = `${this.configService.apiUrl}/relaysExtending`;
    }

    getRelays(): Observable<Relay[]> {
        this.messageService.add('RelayService: fetched relays, ' + this.baseURL);
        return this.http.get<Relay[]>(this.baseURL);
    }

    getRelaysExtending(): Observable<Relay[]> {
        this.messageService.add('RelayService: fetched relaysExtending, ' + this.baseURL);
        return this.http.get<Relay[]>(this.baseURLExtendingRelays);
    }

    getRelay(id: number): Observable<Relay> {
        this.messageService.add(`RelayService: fetched relay id=${id}, ${this.baseURL}`);
        return this.http.get<Relay>(this.baseURL + '/' + id);
    }

    setValue(relay: Relay): Observable<any> {
        this.messageService.add('RelayService: setValue relay, new value: ' + relay.active + ', ' + this.baseURL);
        return this.http.post(`${this.baseURL}`, relay);
    }

    setValueExtending(relay: Relay): Observable<any> {
        this.messageService.add('RelayService: setValue relay, new value: ' + relay.active + ', ' + this.baseURL);
        return this.http.post(`${this.baseURLExtendingRelays}`, relay);
    }
}

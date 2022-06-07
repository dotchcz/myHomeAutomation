import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';

import {Relay} from './relay';
import {MessageService} from "./message.service";

@Injectable({
    providedIn: 'root'
})
export class RelayService {

    private baseURL = `http://10.0.0.135:5266/relays`

    constructor(private http: HttpClient, private messageService: MessageService) {
    }

    getRelays(): Observable<Relay[]> {
        this.messageService.add('RelayService: fetched relays, ' + this.baseURL);
        return this.http.get<Relay[]>(this.baseURL)
    }

    getRelay(id: number): Observable<Relay> {
        this.messageService.add(`RelayService: fetched relay id=${id}, ${this.baseURL}`);
        return this.http.get<Relay>(this.baseURL + '/' + id);
    }

    setValue(relay: Relay): Observable<any> {
        this.messageService.add('RelayService: setValue relay, new value: ' + relay.active + ', ' + this.baseURL);
        return this.http.post(`${this.baseURL}`, relay)
    }
}

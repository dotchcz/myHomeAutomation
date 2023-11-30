// src/app/config.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ConfigService {
    private config: any;

    constructor(private http: HttpClient) {}

    loadConfig() {
        return this.http.get('/assets/config.json').toPromise().then(data => {
            this.config = data;
        });
    }

    get apiUrl() {
        return this.config.apiUrl;
    }
}

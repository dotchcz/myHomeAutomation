import {Component, OnInit} from '@angular/core';

import {Relay} from "../relay";
import {RelayService} from "../relay.service";
import {getTargetsByBuilderName} from "@angular/cdk/schematics";
import {CommonService} from "../common.service";

@Component({
    selector: 'app-relays',
    templateUrl: './relays.component.html',
    styleUrls: ['./relays.component.css']
})
export class RelaysComponent implements OnInit {
    relays: Relay[] = [];
    extendingRelays: Relay[] = [];
    interval: any;

    constructor(private relayService: RelayService, public commonService: CommonService) {
    }

    ngOnInit(): void {
        this.getRelays();
        this.interval = setInterval(() => {
            this.getRelays();
        }, 2000);
    }

    getRelays(): void {
        this.relayService.getRelays()
            .subscribe(relays => this.relays = relays);
        this.relayService.getRelaysExtending()
            .subscribe(relaysE => this.extendingRelays = relaysE);
    }

    onChange(relay: Relay) {
        console.log('value changed for ' + relay.id);
        relay.active = !relay.active;
        this.relayService.setValue(relay)
            .subscribe(response => console.log(response));
    }

    onChangeExtending(relay: Relay) {
        console.log('value changed for ' + relay.id);
        relay.active = !relay.active;
        this.relayService.setValueExtending(relay)
            .subscribe(response => console.log(response));
    }
}

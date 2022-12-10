import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Location} from "@angular/common";

import {RelayService} from "../relay.service";
import {Relay} from "../relay";

@Component({
    selector: 'app-relay-detail',
    templateUrl: './relay-detail.component.html',
    styleUrls: ['./relay-detail.component.css']
})
export class RelayDetailComponent implements OnInit {
    relay: Relay = new Relay(1, '', false, 0, new Date( Date.UTC(0,0,0,0,0,0)));

    constructor(
        private route: ActivatedRoute,
        private relayService: RelayService,
        private location: Location) {
    }

    ngOnInit(): void {
        this.getRelay();
        this.log();
    }

    getRelay(): void {
        const id = Number(this.route.snapshot.paramMap.get('id'));
        console.log('getRelay() for ID:' + id)
        this.relayService.getRelay(id)
            .subscribe(relay => {this.relay = relay;
                console.log(this.relay)});
    }

    onChange() {
        console.log('value changed for ' + this.relay.id);
        this.relay.active = !this.relay.active;
        this.onSubmit();
    }

    onSubmit() {
        console.log('onSubmit(): id:' + this.relay.id + ', active:' + this.relay.active + ', type:' + this.relay.type);
        this.log();
        this.relayService.setValue(this.relay)
            .subscribe(response => console.log(response))
    }

    log() {
        console.log(this.relay);
    }

    goBack(): void {
        this.location.back();
    }
}

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
    relay: Relay = new Relay(1,'',false, 0);
    
    constructor(
        private route: ActivatedRoute,
        private relayService: RelayService,
        private location: Location) {
    }

    ngOnInit(): void {
        this.getRelay();
    }

    getRelay(): void {
        const id = Number(this.route.snapshot.paramMap.get('id'));
        this.relayService.getRelay(id)
            .subscribe(relay => this.relay = relay);
    }

    onSubmit() {
        let data = new Relay(this.relay.id, "AAA", this.relay.active, this.relay.type);
        console.log(this.relay.id +  ', ' + this.relay.active + ', type:'+ this.relay.type);
        console.log(data);
        this.relayService.setValue(data)
            .subscribe(response => console.log(response))
    }

    goBack(): void {
        this.location.back();
    }
}

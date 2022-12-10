import { Component, OnInit } from '@angular/core';

import {Relay} from "../relay";
import {RelayService} from "../relay.service";

@Component({
  selector: 'app-relays',
  templateUrl: './relays.component.html',
  styleUrls: ['./relays.component.css']
})
export class RelaysComponent implements OnInit {
  relays: Relay[] = [];
  interval: any;
  
  constructor(private relayService: RelayService) { }

  ngOnInit(): void {
    this.getRelays();
    this.interval = setInterval(() => {
      this.getRelays();
    }, 2000);
  }

  getRelays(): void{
    this.relayService.getRelays()
        .subscribe(relays=> this.relays = relays);
  }

  onChange(relay: Relay){
    console.log('value changed for ' + relay.id);
    relay.active = !relay.active;
    this.relayService.setValue(relay)
        .subscribe(response => console.log(response));
  }
}

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
  
  constructor(private relayService: RelayService) { }

  ngOnInit(): void {
    this.getRelays();
  }

  getRelays(): void{
    this.relayService.getRelays()
        .subscribe(relays=> this.relays = relays);
  }
}

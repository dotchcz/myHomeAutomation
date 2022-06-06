import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RelaysComponent} from "./relays/relays.component";
import { SensorsComponent} from "./sensors/sensors.component";
import { RelayDetailComponent} from "./relay-detail/relay-detail.component";
import {SensorDetailComponent} from "./sensor-detail/sensor-detail.component";

const routes: Routes = [
  { path: 'relay/detail/:id', component: RelayDetailComponent },
  { path: 'relays', component: RelaysComponent },
  { path: 'sensor/detail/:id', component: SensorDetailComponent },
  { path: 'sensors', component: SensorsComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}

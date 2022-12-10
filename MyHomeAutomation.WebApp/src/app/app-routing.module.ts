import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RelaysComponent} from "./relays/relays.component";
import { SensorsComponent} from "./sensors/sensors.component";
import { RelayDetailComponent} from "./relay-detail/relay-detail.component";
import {SensorDetailComponent} from "./sensor-detail/sensor-detail.component";
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'relay/detail/:id', component: RelayDetailComponent },
  { path: 'relays', component: RelaysComponent },
  { path: 'sensor/detail/:id', component: SensorDetailComponent },
  { path: 'sensors', component: SensorsComponent },
  { path: 'dashboard', component: DashboardComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}

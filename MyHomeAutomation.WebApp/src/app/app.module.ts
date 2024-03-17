import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { MessagesComponent } from './messages/messages.component';

import { AppRoutingModule } from './app-routing.module';
import { SensorsComponent } from './sensors/sensors.component';
import { SensorDetailComponent } from './sensor-detail/sensor-detail.component';
import { RelayDetailComponent } from './relay-detail/relay-detail.component';
import { RelaysComponent } from './relays/relays.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { ConfigService } from './config.service';
import { PhotovoltaicsComponent } from './photovoltaics/photovoltaics.component';
import { SafeFormatPipe } from './safe-format.pipe';

export function initializeApp(configService: ConfigService) {
  return (): Promise<any> => {
    return configService.loadConfig();
  }
}

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
  ],
  declarations: [
    AppComponent,
    MessagesComponent,
    SensorsComponent,
    SensorDetailComponent,
    RelayDetailComponent,
    RelaysComponent,
    DashboardComponent,
    PhotovoltaicsComponent,
    SafeFormatPipe
  ],
  bootstrap: [ AppComponent ],
  providers: [{provide: LocationStrategy, useClass: HashLocationStrategy}, ConfigService, { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [ConfigService], multi: true }]
})
export class AppModule { }

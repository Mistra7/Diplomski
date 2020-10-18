import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainTableComponent } from 'src/app/components/main-table/main-table.component';
import { HostListener  } from "@angular/core";
import { ControlWindowComponent } from './components/control-window/control-window.component';
import { MainWindowComponent } from './components/main-window/main-window.component';
import { LogWindowComponent } from './components/log-window/log-window.component';

@NgModule({
  declarations: [
    AppComponent,
    MainTableComponent,
    ControlWindowComponent,
    MainWindowComponent,
    LogWindowComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

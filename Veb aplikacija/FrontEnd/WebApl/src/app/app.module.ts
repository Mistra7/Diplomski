import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainTableComponent } from 'src/app/components/main-table/main-table.component';
import { HostListener  } from "@angular/core";
import { ControlWindowComponent } from './components/control-window/control-window.component';

@NgModule({
  declarations: [
    AppComponent,
    MainTableComponent,
    ControlWindowComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

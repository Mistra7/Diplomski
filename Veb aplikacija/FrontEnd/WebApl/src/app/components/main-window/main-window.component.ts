import { Component, OnInit, ViewChild } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { LogWindowComponent } from '../log-window/log-window.component';

@Component({
  selector: 'app-main-window',
  templateUrl: './main-window.component.html',
  styleUrls: ['./main-window.component.css']
})
export class MainWindowComponent implements OnInit {

  public currentTime: string = "";
  public startTime: number = new Date().getTime();
  public elapsedTime: string = "";
  canDoAcquisition = false;
  @ViewChild(LogWindowComponent) child: LogWindowComponent;
  constructor() { 
    localStorage.setItem("connected", JSON.stringify(false));
    localStorage.setItem("doAcquisiton", JSON.stringify(this.canDoAcquisition));
    setInterval(() => {
      var passedTime = Math.floor((new Date().getTime() - this.startTime)/1000);
      this.currentTime = this.dateFormatter(new Date());
      this.elapsedTime = (Math.floor(passedTime / 3600)) + ":" + (Math.floor(passedTime / 60) % 60)
      + ":" + (passedTime % 60);
    }, 100)
  }

  ngOnInit(): void {
  }

  dateFormatter(date: Date)
  {
    return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear() + " " + date.getHours() + ":"
    + date.getMinutes() + ":" + date.getSeconds();
  }

  startStop()
  {
    this.canDoAcquisition = !this.canDoAcquisition;
    localStorage.setItem("doAcquisiton", JSON.stringify(this.canDoAcquisition));
  }

  getNotification(point: BasePointItem)
  {
    this.child.makeLogEntery(point);
  }

}

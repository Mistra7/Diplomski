import { Component, Input, OnInit, Output } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { PointType } from 'src/app/enumerations/point-type';

@Component({
  selector: 'app-log-window',
  templateUrl: './log-window.component.html',
  styleUrls: ['./log-window.component.css']
})
export class LogWindowComponent implements OnInit {

  logText: string = "";
  constructor() { }

  ngOnInit(): void {
  }

  makeLogEntery(point: BasePointItem){
    this.logText +=  this.dateFormatter(point.timestamp) + " Point of type: " + this.stringyfiedTypeOfPoint(point.type) + " on address: " + point.address + " recivied value: " + point.rawValue + "nextLine" ;
    this.logText = this.logText.replace("nextLine", "<br>");
  }

  stringyfiedTypeOfPoint(type: PointType){
    return PointType[type];
  }

  dateFormatter(date1: Date)
  {
    var date = new Date(date1);
    return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear() + " " + date.getHours() + ":"
    + date.getMinutes() + ":" + date.getSeconds();
  }

}

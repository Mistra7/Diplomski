import { Component, OnInit } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { DigitalInput } from 'src/app/entities/digital-input';
import { AlarmType } from 'src/app/enumerations/alarm-type';
import { PointType } from 'src/app/enumerations/point-type';
import { DState } from 'src/app/enumerations/dState';
import { AnalogOutput } from 'src/app/entities/analog-output';
import { ConfigItem } from 'src/app/entities/config-item';
//import { HostListener  } from "@angular/core";

@Component({
  selector: 'app-main-table',
  templateUrl: './main-table.component.html',
  styleUrls: ['./main-table.component.css']
})
export class MainTableComponent implements OnInit {

  /*@HostListener("click") onClick()
  {
    console.log("Usao");
  }*/
  PointList : Array<BasePointItem> = new Array<BasePointItem>();
  selectedRow = -1;
  selectedPoint : BasePointItem = new BasePointItem();
  constructor() { }

  ngOnInit(): void {
    var point = new DigitalInput();
    point.address = 1000;
    point.alarm = AlarmType.NO_ALARM;
    point.type = PointType.DIGITAL_INPUT;
    point.name = "Point";
    point.pointId = 1;
    point.rawValue = 0;
    point.timestamp = new Date();
    point.state = DState.OFF;

    this.PointList.push(point);

    var point2 = new AnalogOutput();
    point2.address = 2000;
    point2.alarm = AlarmType.NO_ALARM;
    point2.type = PointType.ANALOG_OUTPUT;
    point2.name = "Point2";
    point2.pointId = 2;
    point2.rawValue = 1500;
    point2.timestamp = new Date();
    point2.eguValue = 1500;
    point2.configItem = new ConfigItem();
    point2.configItem.maxValue = 2000;
    point2.configItem.minValue = 0;
    this.PointList.push(point2);
  }

  stringyfiedStateOfPoint(state: DState): string {
    return DState[state];
  }

  stringyfiedTypeOfPoint(type: PointType){
    return PointType[type];
  }

  stringyfiedAlarmOfPoint(alarm: AlarmType){
    return AlarmType[alarm];
  }

  dateFormatter(date: Date)
  {
    return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear() + " " + date.getHours() + ":"
    + date.getMinutes() + ":" + date.getSeconds();
  }

  clickFunc(index: number, point : BasePointItem )
  {
    this.selectedRow = index;
    this.selectedPoint = point;
  }

  openModal()
  {
    document.getElementById("openModalButton").click();
  }
}

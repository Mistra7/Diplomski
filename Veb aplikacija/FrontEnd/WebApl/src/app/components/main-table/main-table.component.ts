import { Component, OnInit } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { DigitalInput } from 'src/app/entities/digital-input';
import { AlarmType } from 'src/app/enumerations/alarm-type';
import { PointType } from 'src/app/enumerations/point-type';
import { DState } from 'src/app/enumerations/dState';
import { AnalogOutput } from 'src/app/entities/analog-output';
import { ConfigItem } from 'src/app/entities/config-item';
import { PointService } from 'src/app/services/point.service';
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
  connected = false;
  constructor(private pointService: PointService) { }

  ngOnInit(): void {
    setInterval(() => {
      if(!this.connected)
      {
        this.pointService.connectToDCom().subscribe(
          (res : any) => {
            console.log(res);
            this.connected = true;
          },
          err => {
            console.log(err);
          }
        )
      }
    }, 1000);
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

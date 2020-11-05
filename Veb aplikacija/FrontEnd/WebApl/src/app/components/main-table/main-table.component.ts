import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BasePointItem, PointIdentifier } from 'src/app/entities/base-point-item';
import { AlarmType } from 'src/app/enumerations/alarm-type';
import { PointType } from 'src/app/enumerations/point-type';
import { DState } from 'src/app/enumerations/dState';
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
  ConfigList : Array<ConfigItem> = new Array<ConfigItem>();
  selectedRow = -1;
  selectedPoint : BasePointItem = new BasePointItem();
  @Output() notifyParent: EventEmitter<BasePointItem> = new EventEmitter<BasePointItem>();
  connected = false;
  doAcqus = false;
  connectionId;
  acqusitionId;
  constructor(private pointService: PointService) { }

  ngOnInit(): void {
    this.connectionId = setInterval(() => {
      this.connectToServer();
    }, 2000);

    this.acqusitionId = setInterval(() => {
      this.doTheAcqusition();
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

  dateFormatter(date1: Date)
  {
    var date = new Date(date1);
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

  connectToServer(){
    this.connected = JSON.parse(localStorage.getItem("connected"));
    if(!this.connected)
    {
      this.pointService.connectToDCom().toPromise().then(
        (res: any) => {
          console.log(Date.now())
          this.PointList = res.points as Array<BasePointItem>;
          this.ConfigList = res.configItems as Array<ConfigItem>;
          console.log(this.PointList);
          this.connected = true;
          localStorage.setItem("connected", JSON.stringify(this.connected));
        }
      )
      .catch(
        err => {
          alert(err);
        }
      )
      /*this.pointService.connectToDCom().subscribe(
        (res : any) => {
          this.PointList = res.points as Array<BasePointItem>;
          this.ConfigList = res.configItems as Array<ConfigItem>;
          console.log(this.PointList);
          this.connected = true;
          localStorage.setItem("connected", JSON.stringify(this.connected));
        },
        err => {
          console.log(err);
        }
      )*/
    }
  }

  doTheAcqusition(){
    this.doAcqus = JSON.parse(localStorage.getItem("doAcquisiton"));
    var identifiers = new Array<number>();
    if(this.doAcqus){
      this.ConfigList.forEach(c => {
        ++c.secondsPassedSinceLastPoll;
        if(c.acquisitionInterval > 0 && c.secondsPassedSinceLastPoll >= c.acquisitionInterval){
          identifiers.push(c.dataBaseId);
          c.secondsPassedSinceLastPoll = 0;
        }
      });

      if(identifiers.length > 0){
        this.pointService.acqusitate(identifiers).subscribe(
          (res: any) => {
            var points = res as Array<BasePointItem>;
            points.forEach(p => {
              this.updatePoint(p);
            })
          },
          err => {
            alert(err);
          }
        )
      }
    }
  }

  getNotification(point: BasePointItem)
  {
    this.updatePoint(point);
  }

  updatePoint(point: BasePointItem)
  {
    this.PointList.forEach(p => {
      if(p.dataBaseId == point.dataBaseId){
        p.rawValue = point.rawValue;
        p.timestamp = point.timestamp;
        if(p.type == PointType.ANALOG_INPUT || p.type == PointType.ANALOG_OUTPUT){
          p.eguValue = point.eguValue;
        }
        else {
          p.state = point.state;
        }
        this.notifyParent.emit(p);
      }      
    });
  }
}

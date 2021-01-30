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
    this.doAcqus = JSON.parse(localStorage.getItem("doAcquisiton"));
    document.getElementById("openModalButton").click();
  }

  connectToServer(){
    this.connected = JSON.parse(localStorage.getItem("connected"));
    if(!this.connected)
    {
      this.pointService.connectToAMS().toPromise().then(
        (res: any) => {
          console.log(res);
          this.PointList = res.points as Array<BasePointItem>;
          this.ConfigList = res.configItems as Array<ConfigItem>;
          this.connected = true;
          localStorage.setItem("connected", JSON.stringify(this.connected));
          this.updatePoints();
        }
      )
      .catch(
        err => {
          console.log(err);
        }
      )
    }
  }

  doTheAcqusition(){
    this.doAcqus = JSON.parse(localStorage.getItem("doAcquisiton"));
    var identifiers = new Array<number>();
    if(this.doAcqus){
      this.ConfigList.forEach(c => {
        ++c.secondsPassedSinceLastPoll;
        if(c.acquisitionInterval > 0 && c.secondsPassedSinceLastPoll >= c.acquisitionInterval){
          identifiers.push(c.id);
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
            if(err.error == "ConnectionFailiure")
              this.restartConnection();
            else
            {
              console.log(err);
            }
          }
        )
      }
    }
    else 
    {
      this.PointList.forEach(p => {
        if(p.acquPeriod > 0)
        {
          ++p.secsSinceLastAcqu;
          if(p.secsSinceLastAcqu >= p.acquPeriod)
          {
            this.pointService.readRegister(p.pointId).subscribe(
              res => {
                this.updatePoint(res as BasePointItem);
              },
              err => {
                if(err.error == "ConnectionFailiure")
                  this.restartConnection();
              }
            );
            p.secsSinceLastAcqu = 0;
          }
        }
      })
    }
  }

  getNotification(point: BasePointItem)
  {
    if(point == null)
    {
      this.restartConnection();
    }
    else
    {
      this.updatePoint(point);
    }
    
  }

  updatePoint(point: BasePointItem)
  {
    this.PointList.forEach(p => {
      if(p.pointId == point.pointId){
        p.rawValue = point.rawValue;
        p.timestamp = point.timestamp;
        p.alarm = point.alarm;
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

  getNotification1(point: BasePointItem)
  {
    this.PointList.forEach(p => {
      if(p.pointId == point.pointId)
      {
        p.acquPeriod = point.acquPeriod;
        return;
      }
    })
  }

  updatePoints()
  {
    this.PointList.forEach(p => {
      p.acquPeriod = 0;
      p.secsSinceLastAcqu = 0;
    })
  }

  restartConnection()
  {
    localStorage.setItem("doAcquisiton", JSON.stringify(false));
    this.PointList = [];
    this.ConfigList = [];
    localStorage.setItem("connected", JSON.stringify(false));
    alert("Connection aborted! Trying to connect to dCom!")
  }
}

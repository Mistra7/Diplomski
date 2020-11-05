import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { DState } from 'src/app/enumerations/dState';
import { FormsModule } from '@angular/forms';
import { PointType } from 'src/app/enumerations/point-type';
import { PointService } from 'src/app/services/point.service';

@Component({
  selector: 'app-control-window',
  templateUrl: './control-window.component.html',
  styleUrls: ['./control-window.component.css']
})
export class ControlWindowComponent implements OnInit {
  @Input() point: BasePointItem = new BasePointItem();
  @Output() notifyParent: EventEmitter<BasePointItem> = new EventEmitter();
  commandedValue = 0;
  isCommandedValueValid = true;
  constructor(private pointService: PointService) {
  }
  
  ngOnInit(): void {
    console.log(this.point);
  }

  valueOfThePoint(point: BasePointItem) {
    if(point.type == 1 || point.type == 2)
    {
      return DState[point.state];
    }
    else
    {
      return (point.eguValue);
    }
  }

  writeCommand(){
    this.pointService.commandRegister(this.point.dataBaseId, this.commandedValue).subscribe(
      (res: any) => {
        this.notifyParent.emit(res as BasePointItem);
        this.commandedValue = 0;
      },
      err => {
        alert(err.message);
        this.commandedValue = 0;
      }
    )
  }

  valueChanged() {
    if(this.point.type == 1){
      this.checkDigitalValue();
    }
    else {
      this.checkAnalogValue();
    }
  }

  checkAnalogValue() {
    if(this.commandedValue < this.point.minValue || this.commandedValue > this.point.maxValue)
    {
      this.isCommandedValueValid = false;
    }
    else
    {
      this.isCommandedValueValid = true;
    }
  }

  checkDigitalValue() {
    if(this.commandedValue != 0 && this.commandedValue != 1)
    {
      this.isCommandedValueValid = false;
    }
    else
    {
      this.isCommandedValueValid = true;
    }
  }

  readCommand()
  {
    this.pointService.readRegister(this.point.dataBaseId).subscribe(
      (res : any) => {
        this.notifyParent.emit(res as BasePointItem);
      },
      err => 
      {
        alert(err.message);
      }
    )
  }

  

}

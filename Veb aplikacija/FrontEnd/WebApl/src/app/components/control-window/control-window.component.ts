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
  @Input() canDoAcq : boolean = false;
  @Output() notifyParent: EventEmitter<BasePointItem> = new EventEmitter();
  @Output() notifyParent1: EventEmitter<BasePointItem> = new EventEmitter();
  commandedValue = 0;
  acqValue = 0;
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

  setAcqPeriod()
  {
    this.point.acquPeriod = this.acqValue;
    this.acqValue = 0;
  }

  writeCommand(){
    this.pointService.commandRegister(this.point.pointId, this.commandedValue).subscribe(
      (res: any) => {
        this.notifyParent.emit(res as BasePointItem);
        this.commandedValue = 0;
      },
      err => {
        if(err.error = "ConnectionFailiure")
          this.notifyParent.emit(null);
        else
          console.log(err);
        this.commandedValue = 0;
        document.getElementById("closeModal").click();
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
    this.pointService.readRegister(this.point.pointId).subscribe(
      (res : any) => {
        this.notifyParent.emit(res as BasePointItem);
      },
      err => 
      {
        if(err.error = "ConnectionFailiure")
          this.notifyParent.emit(null);
        else
          console.log(err);
        this.commandedValue = 0;
        document.getElementById("closeModal").click();
      }
    )
  }

  checkValue(value : number)
  {
    if(value < 0)
      return true;
    else
      return false;
  }

  

}

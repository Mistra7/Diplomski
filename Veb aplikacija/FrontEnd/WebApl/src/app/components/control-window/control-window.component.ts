import { Component, OnInit, Input } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { AnalogBase } from 'src/app/entities/analog-base';
import { DigitalBase } from 'src/app/entities/digital-base';
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
      return DState[(point as DigitalBase).state];
    }
    else
    {
      return (point as AnalogBase).eguValue;
    }
  }

  writeCommand(){
    this.pointService.commandRegister(this.point.pointId, this.commandedValue).subscribe(
      (res: any) => {
        console.log(res);
      },
      err => {
        alert(err.message);
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
    if(this.commandedValue < this.point.configItem.minValue || this.commandedValue > this.point.configItem.maxValue)
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
        console.log(res);
      },
      err => 
      {
        console.log(err);
      }
    )
  }

  

}

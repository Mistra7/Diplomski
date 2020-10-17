import { Component, OnInit, Input } from '@angular/core';
import { BasePointItem } from 'src/app/entities/base-point-item';
import { AnalogBase } from 'src/app/entities/analog-base';
import { DigitalBase } from 'src/app/entities/digital-base';
import { DState } from 'src/app/enumerations/dState';
import { FormsModule } from '@angular/forms';
import { PointType } from 'src/app/enumerations/point-type';

@Component({
  selector: 'app-control-window',
  templateUrl: './control-window.component.html',
  styleUrls: ['./control-window.component.css']
})
export class ControlWindowComponent implements OnInit {
  @Input() point: BasePointItem = new BasePointItem();
  commandedValue = 0;
  isCommandedValueValid = true;
  constructor() {
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
    console.log(this.commandedValue);
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
    //ubaciti da sa servera trazi min i maks vrijednost
    if(this.commandedValue < 0 || this.commandedValue > 1000)
    {
      this.isCommandedValueValid = false;
    }
    else
    {
      this.isCommandedValueValid = true;
    }
  }

  checkDigitalValue() {
    //ne treba traziti sa servera jer je za digitalne uvijek vrijednost ili 1 ili 0
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

  }

  

}

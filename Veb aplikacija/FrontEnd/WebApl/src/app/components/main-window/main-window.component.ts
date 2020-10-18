import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-window',
  templateUrl: './main-window.component.html',
  styleUrls: ['./main-window.component.css']
})
export class MainWindowComponent implements OnInit {

  public currentTime: string = "";
  public startTime: number = new Date().getTime();
  public elapsedTime: string = "";
  constructor() { 
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

}

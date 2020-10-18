import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PointService {
  readonly BaseURI = 'http://localhost:40000/api'
  constructor(private http: HttpClient) { }

  connectToDCom() {
    return this.http.get(this.BaseURI + "/Point/Connect");
  }

  readRegister(pointId: number)
  {
    return this.http.get(this.BaseURI + "/Point/ReadSingleRegister/?pid=" + pointId);
  }

  commandRegister(pointId: number, value: number)
  {
    return this.http.get(this.BaseURI + "/Point/CommandSingleRegister/?pid=" + pointId + "&value=" + value);
  }

  getMinAndMaxValue(pointId: number)
  {
    return this.http.get(this.BaseURI + "/Point/GetMinAndMaxValue/?pid=" + pointId);
  }
}

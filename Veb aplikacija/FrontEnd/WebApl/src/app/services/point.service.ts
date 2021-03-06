import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PointIdentifier } from '../entities/base-point-item';

@Injectable({
  providedIn: 'root'
})
export class PointService {
  readonly BaseURI = 'http://localhost:40000/api'
  constructor(private http: HttpClient) { }

  connectToAMS() {
    return this.http.get(this.BaseURI + "/Point/Connect");
  }

  readRegister(pointId: number)
  {
    return this.http.get(this.BaseURI + "/Point/ReadSingleRegister?pid=" + pointId);
  }

  commandRegister(pointId: number, value: number)
  {
    return this.http.get(this.BaseURI + "/Point/CommandSingleRegister?pid=" + pointId + "&value=" + value);
  }

  acqusitate(identifiers: Array<number>)
  {
    return this.http.post(this.BaseURI + "/Point/DoTheAcqusition", identifiers);
  }
}

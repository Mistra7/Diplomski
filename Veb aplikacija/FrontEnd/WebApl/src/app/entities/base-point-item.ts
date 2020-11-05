import {AlarmType} from 'src/app/enumerations/alarm-type';
import {PointType} from 'src/app/enumerations/point-type';
import { DState } from '../enumerations/dState';
import { ConfigItem } from './config-item';

export class BasePointItem {
    type: PointType;
    address: number;
    timestamp: Date;
    name: string;
    rawValue: number;
    commandedValue: number;
    alarm: AlarmType;
    pointId: number;    
    dataBaseId: number;
    minValue: number;
    maxValue: number;
    eguValue: number;
    state: DState;

    constructor() {
        this.name = "";
    }
}

export class PointIdentifier {
    PointType: PointType;
    Address: number;

    constructor(type: PointType, addr: number)
    {
        this.PointType = type;
        this.Address = addr;
    }
}

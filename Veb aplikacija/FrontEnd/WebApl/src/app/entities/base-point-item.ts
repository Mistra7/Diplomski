import {AlarmType} from 'src/app/enumerations/alarm-type';
import {PointType} from 'src/app/enumerations/point-type';

export class BasePointItem {
    pointId: number;
    type: PointType;
    address: number;
    timestamp: Date;
    name: string;
    rawValue: number;
    commandedValue: number;
    alarm: AlarmType;

    constructor() {
        this.name = "";
    }
}

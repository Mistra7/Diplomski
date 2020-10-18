import {AlarmType} from 'src/app/enumerations/alarm-type';
import {PointType} from 'src/app/enumerations/point-type';
import { ConfigItem } from './config-item';

export class BasePointItem {
    pointId: number;
    type: PointType;
    address: number;
    timestamp: Date;
    name: string;
    rawValue: number;
    commandedValue: number;
    alarm: AlarmType;
    configItem: ConfigItem;

    constructor() {
        this.name = "";
    }
}

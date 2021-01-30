import { PointType } from '../enumerations/point-type';

export class ConfigItem {
    registryType : PointType;
    numberOfRegisters : number;
    startAddress : number;
    acquisitionInterval : number;
    id : number;
    secondsPassedSinceLastPoll : number;

    constructor() {}
}
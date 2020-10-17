import {BasePointItem} from 'src/app/entities/base-point-item'
import { DState } from '../enumerations/dState';

export class DigitalBase extends BasePointItem {
    state: DState;

    constructor()
    {
        super();
    }
}
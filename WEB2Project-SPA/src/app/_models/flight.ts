import { Destination } from './destination';

export interface Flight { 
    id: number;
    departureTime: Date;
    arrivalTime: Date;
    departureDestination: Destination;
    arrivalDestination: Destination;
}

import { Destination } from './destination';

export interface Flight { 
    id: number;
    departureTime: Date;
    arrivalTime: Date;
    departureDestination: Destination;
    travelLength: number;
    transitLocation: string;
    arrivalDestination: Destination;
    price: number;
}

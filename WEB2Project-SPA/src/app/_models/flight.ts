import { Destination } from './destination';

export interface Flight { 
    id: number;
    departureTime: Date;
    arrivalTime: Date;
    departureDestination: Destination;
    arrivalDestination: Destination;
    ticketPrice: number;
    mileage: number;
    travelTime: number;
}

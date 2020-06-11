import { Destination } from './destination';

export interface Flight { 
    id: number;
    departureTime: Date;
    arrivalTime: Date;
    departureDestination: Destination;
    travelLength: number;
    transitLocation: string;
    arrivalDestination: Destination;
    ticketPrice: number;
    mileage: number;
    travelTime: number;
    luggage: number;
    averageGrade: number;
    companyId: number;
}

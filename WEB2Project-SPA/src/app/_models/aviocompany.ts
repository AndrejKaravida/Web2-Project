import { Flight } from './flight';
import { Destination } from './destination';

export interface AvioCompany { 
    id: number;
    name: string;
    address: string;
    promoDescription: string;
    destinations: Destination[];
    averageGrade: number;
    photo: string;
    flights: Flight[];
}

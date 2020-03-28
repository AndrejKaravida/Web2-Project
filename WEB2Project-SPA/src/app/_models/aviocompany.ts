import { Flight } from './flight';

export interface AvioCompany { 
    id: number;
    name: string;
    address: string;
    promoDescription: string;
    averageGrade: number;
    photo: string;
    flights: Flight[];
}

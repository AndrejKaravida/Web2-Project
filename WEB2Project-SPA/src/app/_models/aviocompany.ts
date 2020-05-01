import { Flight } from './flight';
import { Destination } from './destination';

export interface AvioCompany {
    id: number;
    name: string;
    headOffice: Destination;
    promoDescription: string;
    companyDestinations: Destination[];
    averageGrade: number;
    photo: string;
    flights: Flight[];
}

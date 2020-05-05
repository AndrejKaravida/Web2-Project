import { Flight } from './flight';
import { Destination } from './destination';
import { User } from './_userModels/user';

export interface AvioCompany {
    id: number;
    name: string;
    headOffice: Destination;
    promoDescription: string;
    companyDestinations: Destination[];
    averageGrade: number;
    photo: string;
    flights: Flight[];
    admin: User;
}

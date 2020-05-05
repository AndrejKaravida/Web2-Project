import { Flight } from './flight';
import { Destination } from './destination';
import { User } from '../_userModels/user';
import { Branch } from '../_shared/branch';

export interface AvioCompany {
    id: number;
    name: string;
    headOffice: Branch;
    promoDescription: string;
    companyDestinations: Destination[];
    averageGrade: number;
    photo: string;
    flights: Flight[];
    admin: User;
}

import { Vehicle } from './vehicle';
import { User } from '../_userModels/user';
import { Branch } from '../_shared/branch';

export interface CarCompany{ 
    id: number;
    name: string;
    address: string;
    promoDescription: string;
    averageGrade: number;
    vehicles: Vehicle[];
    weekRentalDiscount: number;
    monthRentalDiscount: number;
    branches: Branch[];
    photo: string;
    headOffice: Branch;
    admin: User;
    rowVersion: any;
}


import { Vehicle } from './vehicle';
import { Destination } from './destination';

export interface CarCompany{ 
    id: number;
    name: string;
    address: string;
    promoDescription: string;
    averageGrade: number;
    vehicles: Vehicle[];
    weekRentalDiscount: number;
    monthRentalDiscount: number;
    destinations: Destination[];
    photo: string;
    headOffice: Destination;
}


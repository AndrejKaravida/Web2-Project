import { Vehicle } from './vehicle';
import { Location } from './location';

export interface CarCompany{ 
    id: number;
    name: string;
    address: string;
    promoDescription: string;
    averageGrade: number;
    vehicles: Vehicle[];
    weekRentalDiscount: number;
    monthRentalDiscount: number;
    locations: Location[];
}


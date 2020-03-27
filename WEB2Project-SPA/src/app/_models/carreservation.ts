import { Vehicle } from './vehicle';

export interface Reservation {
    id: number;
    vehicle: Vehicle;
    companyName: string;
    startDate: Date;
    endDate: Date;
    status: string;
    totalPrice: number;
    numberOfDays: number;
    daysLeft: number;
}

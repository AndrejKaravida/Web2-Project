import { Vehicle } from './vehicle';

export interface Reservation {
    id: number;
    vehicle: Vehicle;
    companyName: string;
    companyId: number;
    startDate: Date;
    endDate: Date;
    status: string;
    totalPrice: number;
    numberOfDays: number;
    daysLeft: number;
}

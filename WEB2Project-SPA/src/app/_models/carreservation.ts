import { Vehicle } from './vehicle';

export interface Reservation {
    id: number;
    vehicle: Vehicle;
    startDate: Date;
    endDate: Date;
    status: string;
    totalPrice: number;
    numberOfDays: number;
}

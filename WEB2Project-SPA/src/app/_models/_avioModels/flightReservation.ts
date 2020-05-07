export interface FlightReservation { 
    departureDestination: string;
    arrivalDestination: string;
    departureDate: Date;
    arrivalDate: Date;
    price: number;
    travelLength: number;
    status: string;
}
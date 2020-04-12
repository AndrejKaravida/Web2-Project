export interface User {
    id: number;
    email: string;
    name: string;
    surname: string;
    userName: string;
    age: number;
    gender: string;
    city: string;
    country: string;
    roles?: string[];
}
export interface User {
    id: number;
    userName: string;
    age: number;
    gender: string;
    city: string;
    country: string;
    roles?: string[];
}
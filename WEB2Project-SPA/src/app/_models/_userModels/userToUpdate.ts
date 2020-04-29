import { UserMetadata } from './userMetadata';

export interface UserToUpdate {
    email: string;
    user_metadata: UserMetadata;
}